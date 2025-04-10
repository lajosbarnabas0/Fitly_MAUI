using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
    public partial class CalorieViewModel : ObservableObject
    {
        [ObservableProperty]
        User selectedUser;

        [ObservableProperty]
        List<Meal>? meals;

        [ObservableProperty]
        public Meal selectedMeal;

        private Dictionary<Meal, double> mealGrams = new();
        public Meal Meal { get; set; }
        public string Grams { get; set; }

        [ObservableProperty]
        public ObservableCollection<Meal> selectedMeals;

        public double Progress => SelectedUser != null && SelectedUser.recommended_calories > 0
            ? Math.Min(1.0, TotalCalories / SelectedUser.recommended_calories.Value)
            : 0;

        public Color ProgressColor => Progress >= 1.0 ? Colors.Red : Colors.Green;

        public string CalorieSummary => SelectedUser != null && SelectedUser.recommended_calories.HasValue
            ? $"{SelectedUser.recommended_calories.Value} / {Math.Round(TotalCalories)}"
            : $"0 / {Math.Round(TotalCalories)}";

        public double RemainingCalories => SelectedUser != null
            ? Math.Max(0, (int)(SelectedUser.recommended_calories - TotalCalories))
            : 0;

        public double TotalCalories => SelectedMeals.Sum(meal =>
        {
            if (mealGrams.TryGetValue(meal, out double grams))
            {
                return (meal.kcal / 100.0) * grams;
            }
            return 0;
        });


        public CalorieViewModel()
        {
            SelectedMeals = new ObservableCollection<Meal>();
            SelectedMeals.CollectionChanged += (s, e) =>
            {
                OnPropertyChanged(nameof(TotalCalories));
                OnPropertyChanged(nameof(RemainingCalories));
                OnPropertyChanged(nameof(Progress));
                OnPropertyChanged(nameof(ProgressColor));
                OnPropertyChanged(nameof(CalorieSummary));
                OnPropertyChanged(nameof(TotalCalories));    // Frissítjük a kalóriát
                OnPropertyChanged(nameof(TotalFatFormatted));         // Frissítjük a zsírt
                OnPropertyChanged(nameof(TotalCarbsFormatted));       // Frissítjük a szénhidrátot
                OnPropertyChanged(nameof(TotalProteinFormatted));     // Frissítjük a fehérjét
                OnPropertyChanged(nameof(TotalSaltFormatted));        // Frissítjük a sót
                OnPropertyChanged(nameof(TotalSugarFormatted));
            };
        }

        [RelayCommand]
        public void AddMeal()
        {
            if (SelectedMeal != null && double.TryParse(Grams, out double gramValue) && gramValue > 0)
            {
                if (!SelectedMeals.Contains(SelectedMeal))
                {
                    SelectedMeals.Add(SelectedMeal);
                }

                mealGrams[SelectedMeal] = gramValue;
                Grams = "";
            }
        }

        [RelayCommand]
        public void RemoveMeal(Meal meal)
        {
            if (meal != null && SelectedMeals.Contains(meal))
            {
                SelectedMeals.Remove(meal);
            }
        }

        [RelayCommand]
        async Task Appearing()
        {
            string? userID = await SecureStorage.Default.GetAsync("UserId");
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
                        Console.WriteLine("Hiba történt");
                        return;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Hiba történt az Appearing parancsban: {ex.Message}");
                    Debug.WriteLine($"StackTrace: {ex.StackTrace}");
                    return;
                }
            }
            else
            {
                await Shell.Current.DisplayAlert("Figyelmeztetés", "Ahhoz, hogy tudjunk ajánlott kalóriát számolni, be kell jelentkeznie!", "Ok");
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
        }

        // Kiszámítja az összesített zsírtartalmat
        public string TotalFatFormatted => $"{SelectedMeals.Sum(meal => meal.fat).ToString("F2")} g";

        // Kiszámítja az összesített szénhidrátot
        public string TotalCarbsFormatted => $"{SelectedMeals.Sum(meal => meal.carb).ToString("F2")} g";

        // Kiszámítja az összesített fehérjét
        public string TotalProteinFormatted => $"{SelectedMeals.Sum(meal => meal.protein).ToString("F2")} g";

        // Kiszámítja az összesített sótartalmat
        public string TotalSaltFormatted => $"{SelectedMeals.Sum(meal => meal.salt).ToString("F2")} g";

        // Kiszámítja az összesített cukortartalmat
        public string TotalSugarFormatted => $"{SelectedMeals.Sum(meal => meal.sugar).ToString("F2")} g";
    }
}
