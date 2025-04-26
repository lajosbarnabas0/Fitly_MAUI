using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http.Headers;
using System.Text.Json;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Fitly.API;
using Fitly.Models;
namespace Fitly.ViewModels
{
    public partial class ProfileViewModel : ObservableObject, INotifyPropertyChanged
    {

        [ObservableProperty]
        bool isReadOnly = true;

        [ObservableProperty]
        User selectedUser;

        [ObservableProperty]
        User originalUser;

        public FileResult SelectedImageFile { get; private set; }

        // Enum szöveges értékei a Pickerhez
        public ObservableCollection<LoseOrGain> LoseOrGainOptions { get; } = new()
        {
            LoseOrGain.Fogyás,
            LoseOrGain.Hízás
        };


        [RelayCommand]
        public async Task AddImages()
        {
            try
            {
                var result = await MediaPicker.PickPhotoAsync();
                if (result != null)
                {
                    SelectedImageFile = result;
                    Shell.Current?.DisplayAlert("Feltöltés", "A kép feltöltése sikeres!", "Ok");
                }
            }
            catch (Exception ex)
            {
                Shell.Current?.DisplayAlert("Feltöltés", "A kép feltöltése sikertelen!", "Ok");
                Console.WriteLine($"Hiba kép kiválasztásnál: {ex.Message}");
                return;
            }
        }

        [RelayCommand]
        async Task SaveAvatar()
        {
            try
            {
                if (SelectedImageFile == null)
                {
                    Console.WriteLine("Nincs kiválasztott fájl.");
                    return;
                }

                // Létrehozzuk a HttpClient példányt
                using var httpClient = new HttpClient();

                // Hozzáadjuk a szükséges headereket
                httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64)");
                var token = await SecureStorage.Default.GetAsync("LoginToken");
                if (!string.IsNullOrEmpty(token))
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }

                // Létrehozzuk a MultipartFormDataContent-et
                using var formData = new MultipartFormDataContent();

                // Fájl hozzáadása form-data-hoz
                var fileStream = await SelectedImageFile.OpenReadAsync();
                var streamContent = new StreamContent(fileStream);
                streamContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
                formData.Add(streamContent, "avatar", SelectedImageFile.FileName);

                // Küldés az API végpontra
                var url = "https://bgs.jedlik.eu/hm/backend/public/api/user/avatar";
                var response = await httpClient.PostAsync(url, formData);

                // Ellenőrizzük a választ
                if (response.IsSuccessStatusCode)
                {
                    Shell.Current?.DisplayAlert("Siker", "A bejegyzés mentése sikeres volt!", "Ok");
                    Console.WriteLine("Fájl sikeresen feltöltve!");
                }
                else
                {
                    Console.WriteLine($"Hiba történt: {response.StatusCode}");
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    Shell.Current?.DisplayAlert("Hiba", "Hiba történt!", "Ok");
                    Console.WriteLine($"Hibaüzenet: {errorMessage}");
                }
            }
            catch (Exception ex)
            {
                Shell.Current?.DisplayAlert("Hiba", "Hiba történt!", "Ok");
                Console.WriteLine($"Hiba fájlfeltöltés közben: {ex.Message}");
                return;
            }

        }


        private readonly string url = "https://bgs.jedlik.eu/hm/backend/public/api/users";

        [RelayCommand]
        async Task Appearing()
        {
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;

            if (!(accessType == NetworkAccess.Internet))
            {
                await Shell.Current.DisplayAlert("Hiba", "Kérjük csatlakozzon az internethez!", "Ok");
            }

            string? isLoginSet = SecureStorage.Default.GetAsync("LoginToken").Result;
            string? userID = SecureStorage.Default.GetAsync("UserId").Result;

            if (isLoginSet != null)
            {
                try
                {
                    User? user = await GetData.GetUserById($"{url}/{userID}");
                    if (user != null)
                    {
                        string userJson = JsonSerializer.Serialize(user);
                        Preferences.Set("UserData", userJson);
                        SelectedUser = user;
                        OriginalUser = user;

                    }
                    else
                    {
                        return;
                    }
                }
                catch (Exception ex)
                {
                    return;
                }
            }
        }

        [RelayCommand]
        async Task Logout()
        {
            SecureStorage.RemoveAll();
            await Shell.Current.GoToAsync("//MainPage");
        }

        [RelayCommand]

        async Task editButton()
        {
            IsReadOnly = false;
            OriginalUser = JsonSerializer.Deserialize<User>(JsonSerializer.Serialize(SelectedUser));
        }

        [RelayCommand]
        async Task cancelButton()
        {
            IsReadOnly = true;
            SelectedUser.weight = OriginalUser.weight;
            SelectedUser.height = OriginalUser.height;
            SelectedUser.lose_or_gain = OriginalUser.lose_or_gain;
            SelectedUser.goal_weight = OriginalUser.goal_weight;

            OnPropertyChanged(nameof(SelectedUser));
        }

        [RelayCommand]
        async Task saveButton()
        {
            if (IsUserDataChanged())
            {
                CalculateCalorie();
                UpdateWeightGoalStatus();
                string url = "https://bgs.jedlik.eu/hm/backend/public/api/users/profile";
                var requestData = new User
                {
                    weight = SelectedUser.weight,
                    height = SelectedUser.height,
                    lose_or_gain = SelectedUser.lose_or_gain,
                    goal_weight = SelectedUser.goal_weight,
                    recommended_calories = SelectedUser.recommended_calories,
                    birthday = SelectedUser.birthday
                };

                var response = await SendData.UpdateProfile(url, requestData);

                if (response != null)
                {
                    if (response != null)
                    {
                        Console.WriteLine(response.message);
                    }
                    else
                    {
                        await Shell.Current.DisplayAlert("Hiba", "Sikertelen küldés", "Ok");
                    }
                }
                else
                {
                    await Shell.Current.DisplayAlert("Hiba", "Hiba történt!", "Ok");
                }
                IsReadOnly = true;
                OnPropertyChanged(nameof(SelectedUser));
                OriginalUser = SelectedUser;
            }
            else
            {
                await Shell.Current.DisplayAlert("Hiba", "Nem történt változtatás az adatokban!", "Ok");
            }
        }

        private bool IsUserDataChanged()
        {
            return SelectedUser.weight != OriginalUser.weight || SelectedUser.height != OriginalUser.height || SelectedUser.lose_or_gain != OriginalUser.lose_or_gain || SelectedUser.goal_weight != OriginalUser.goal_weight || SelectedUser.recommended_calories != OriginalUser.recommended_calories;
        }

        async Task CalculateCalorie()
        {
            if (SelectedUser.weight == null || SelectedUser.height == null || SelectedUser.lose_or_gain == null || SelectedUser.birthday == "" || SelectedUser.gender == null)
            {
                Shell.Current?.DisplayAlert("Hiányos adatok", "Kérjük töltse ki a profil oldalon az adatait, hogy ki tudjuk számítani az ajánlott kalóriát!", "Ok");
                return;
            }
            CalculateRecommendedCalorie();
        }

        private async void CalculateRecommendedCalorie()
        {
            // Ellenőrizzük, hogy a szükséges adatok elérhetők-e
            if (string.IsNullOrWhiteSpace(SelectedUser.birthday) ||
                SelectedUser.height == null || SelectedUser.weight == null)
            {
                SelectedUser.recommended_calories = null;
                return;
            }

            // Próbáljuk meg parse-olni a születési dátumot
            if (!DateTime.TryParse(SelectedUser.birthday, out DateTime birthDate))
            {
                SelectedUser.recommended_calories = null;
                return;
            }

            int age = DateTime.Now.Year - birthDate.Year;
            if (DateTime.Now < birthDate.AddYears(age))
                age--;

            double bmr;

            switch (SelectedUser.GenderEnum)
            {
                case Gender.male:
                    bmr = 88.362 + (13.397 * SelectedUser.weight.Value) + (4.799 * SelectedUser.height.Value) - (5.677 * age);
                    break;
                case Gender.female:
                    bmr = 447.593 + (9.247 * SelectedUser.weight.Value) + (3.098 * SelectedUser.height.Value) - (4.330 * age);
                    break;
                default:
                    SelectedUser.recommended_calories = null;
                    return;
            }

            SelectedUser.recommended_calories = Math.Round(bmr);
        }

        private void UpdateWeightGoalStatus()
        {
            if (SelectedUser?.weight == null || SelectedUser?.goal_weight == null) return;

            SelectedUser.LoseOrGainEnum = SelectedUser.goal_weight < SelectedUser.weight
                ? LoseOrGain.Fogyás
                : LoseOrGain.Hízás;
        }
    }
}
