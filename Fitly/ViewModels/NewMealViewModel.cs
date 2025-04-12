using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Fitly.API;
using Fitly.Models;

namespace Fitly.ViewModels
{
    public partial class NewMealViewModel : ObservableObject
    {

        [ObservableProperty]
        Meal newMeal = new Meal();

        [RelayCommand]
        async Task SaveMeal()
        {
            string url = "https://bgs.jedlik.eu/hm/backend/public/api/meals";
            var requestData = new Meal
            {
                kcal = NewMeal.kcal,
                name = NewMeal.name,
                protein = NewMeal.protein,
                sugar = NewMeal.sugar,
                salt = NewMeal.salt,
                fat = NewMeal.fat,
                carb = NewMeal.carb

            };

            var response = await SendData.SendMeal(url, requestData);

            if (response != null)
            {
                if (response != null)
                {
                    await Shell.Current.DisplayAlert("Információ", "Sikeres mentés", "Ok");
                    newMeal = new();
                }
                else
                {
                    await Shell.Current.DisplayAlert("Hiba", "Sikertelen mentés", "Ok");
                }
            }
            else
            {
                await Shell.Current.DisplayAlert("Hiba", "Hiba történt!", "Ok");
            }
        }
    }
}
