using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Fitly.API;
using Fitly.Models;
using Fitly.Pages;
using static System.Net.WebRequestMethods;

namespace Fitly.ViewModels
{
    public partial class CalorieViewModel : ObservableObject
    {
        [ObservableProperty]
        User selectedUser;

        [ObservableProperty]
        List<Meal>? meals;

        [RelayCommand]
        async Task Appearing()
        {
            string? userID = SecureStorage.Default.GetAsync("UserId").Result;
            var isLoginSet = await SecureStorage.Default.GetAsync("LoginToken");

            if (isLoginSet != null)
            {
                try
                {
                    User? user = await GetData.GetUserById($"{"https://bgs.jedlik.eu/hm/backend/public/api/users"}/{userID}");
                    Meals = await GetData.GetMeals("https://bgs.jedlik.eu/hm/backend/public/api/meals");
                    if (user != null)
                    {
                        string userJson = JsonSerializer.Serialize(user);
                        Preferences.Set("UserData", userJson);
                        SelectedUser = user;

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
            else
            {
                await Shell.Current?.DisplayAlert("Figyelmeztetés", "Ahhoz, hogy tudjunk ajánlott kalóriát számolni, be kell jelentkeznie!", "Ok");
                await Shell.Current.GoToAsync("//LoginPage");
            }
        }

        [RelayCommand]
        async Task CalculateCalorie()
        {
            if (SelectedUser.weight == null || SelectedUser.height == null || SelectedUser.lose_or_gain == null || SelectedUser.birthday == "" || SelectedUser.gender == null)
            {
                Shell.Current?.DisplayAlert("Hiányos adatok", "Kérjük töltse ki a profil oldalon az adatait, hogy ki tudjuk számítani az ajánlott kalóriát!", "Ok");
                Shell.Current?.GoToAsync(nameof(ProfilePage));
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
            

            try
            {
                string url = "https://bgs.jedlik.eu/hm/backend/public/api/users/profile";
                var requestData = new User
                {
                    recommended_calories = SelectedUser.recommended_calories,
                    weight = SelectedUser.weight,
                    height = SelectedUser.height,
                    lose_or_gain = SelectedUser.lose_or_gain,
                    goal_weight = SelectedUser.goal_weight,
                    birthday = SelectedUser.birthday,
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
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
