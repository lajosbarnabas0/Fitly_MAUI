using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Fitly.API;
using Fitly.Models;
using Fitly.Pages;
namespace Fitly.ViewModels
{
    public partial class ProfileViewModel : ObservableObject, INotifyPropertyChanged
    {

        [ObservableProperty]
        bool isReadOnly = true;

        [ObservableProperty]
        bool isPickerEnabled = false;

        [ObservableProperty]
        User selectedUser;

        [ObservableProperty]
        User originalUser;

        // Enum szöveges értékei a Pickerhez
        public ObservableCollection<LoseOrGain> LoseOrGainOptions { get; } = new()
        {
            LoseOrGain.Fogyás,
            LoseOrGain.Hízás
        };



        private readonly string url = "https://bgs.jedlik.eu/hm/backend/public/api/users";

        [RelayCommand]
        async Task Appearing()
        {
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
            IsPickerEnabled = true;
            OriginalUser = JsonSerializer.Deserialize<User>(JsonSerializer.Serialize(SelectedUser));
        }

        [RelayCommand]
        async Task cancelButton()
        {
            IsReadOnly = true;
            IsPickerEnabled = false;
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
                IsPickerEnabled = false;
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
    }
}
