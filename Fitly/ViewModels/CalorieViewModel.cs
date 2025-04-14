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
        ObservableCollection<Meal>? meals;

        [ObservableProperty]
        public Meal selectedMeal;

        private Dictionary<Meal, double> mealGrams = new();
        public Meal Meal { get; set; }
        public string Grams { get; set; }

        private bool isUserDataValid = false;

        [ObservableProperty]
        public ObservableCollection<Meal> selectedMeals;

        public string CalorieSummary => $"{SelectedUser?.recommended_calories ?? 0} / {Math.Round((double)TotalCalories)}";


        public double? Progress => SelectedUser != null && SelectedUser.recommended_calories > 0
            ? Math.Min(1.0, (double)TotalCalories / SelectedUser.recommended_calories.Value)
            : 0;

        public Color ProgressColor => Progress >= 1.0 ? Colors.Red : Colors.Green;

        public double? TotalCalories => SelectedMeals.Sum(meal =>
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
                UpdateValues();
            };
        }

        [RelayCommand]
        async Task ResetMeals() 
        {
            SelectedMeals.Clear();
        }

        [RelayCommand]
        public async Task AddMeal()
        {
            if(Grams == null)
            {
                await Shell.Current.DisplayAlert("Hiba", "Kérjük adja meg a gramm értéket!", "Ok");
                return;
            }
            if (SelectedMeal != null && double.TryParse(Grams, out double gramValue) && gramValue > 0)
            {
                if (!SelectedMeals.Contains(SelectedMeal))
                {
                    SelectedMeals.Add(SelectedMeal);
                }

                mealGrams[SelectedMeal] = gramValue;
                Grams = "";
                UpdateValues();
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
        async Task NewMeal()
        {
            await Shell.Current.GoToAsync(nameof(NewMealPage));
        }

        [RelayCommand]
        async Task Appearing()
        {
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;

            if (accessType == NetworkAccess.Internet)
            {
                return;
            }
            else
            {
                await Shell.Current.DisplayAlert("Hiba", "Kérjük csatlakozzon az internethez!", "Ok");
                return;
            }

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
                        if(SelectedUser != null)
                        {
                            CheckData(SelectedUser);
                        }
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

        public async Task CheckData(User user)
        {
            if (!isUserDataValid && (user.weight == null || user.height == null || user.lose_or_gain == null || user.birthday == "" || user.gender == null))
            {
                await Shell.Current.DisplayAlert("Hiányos adatok", "Kérjük töltse ki a profil oldalon az adatait, hogy ki tudjuk számítani az ajánlott kalóriát!", "Ok");
                return;
            }
            else
            {
                isUserDataValid = true; // Az adatok érvényesek
            }
        }

        public string TotalFatFormatted => $"{SelectedMeals.Sum(meal =>
        {
            if (mealGrams.TryGetValue(meal, out double grams))
            {
                return (meal.fat / 100.0) * grams; 
            }
            return 0;
        }):F2} g";

        public string TotalCarbsFormatted => $"{SelectedMeals.Sum(meal =>
        {
            if (mealGrams.TryGetValue(meal, out double grams))
            {
                return (meal.carb / 100.0) * grams;
            }
            return 0;
        }):F2} g";

        public string TotalProteinFormatted => $"{SelectedMeals.Sum(meal =>
        {
            if (mealGrams.TryGetValue(meal, out double grams))
            {
                return (meal.protein / 100.0) * grams;
            }
            return 0;
        }):F2} g";

        public string TotalSaltFormatted => $"{SelectedMeals.Sum(meal =>
        {
            if (mealGrams.TryGetValue(meal, out double grams))
            {
                return (meal.salt / 100.0) * grams;
            }
            return 0;
        }):F2} g";

        public string TotalSugarFormatted => $"{SelectedMeals.Sum(meal =>
        {
            if (mealGrams.TryGetValue(meal, out double grams))
            {
                return (meal.sugar / 100.0) * grams;
            }
            return 0;
        }):F2} g";

        private void UpdateValues()
        {
            OnPropertyChanged(nameof(TotalCalories));
            OnPropertyChanged(nameof(Progress));
            OnPropertyChanged(nameof(ProgressColor));
            OnPropertyChanged(nameof(CalorieSummary));
            OnPropertyChanged(nameof(TotalCalories));
            OnPropertyChanged(nameof(TotalFatFormatted));
            OnPropertyChanged(nameof(TotalCarbsFormatted));
            OnPropertyChanged(nameof(TotalProteinFormatted));
            OnPropertyChanged(nameof(TotalSaltFormatted));
            OnPropertyChanged(nameof(TotalSugarFormatted));
        }

        partial void OnSelectedUserChanged(User value)
        {
            UpdateValues();
        }
    }
}
