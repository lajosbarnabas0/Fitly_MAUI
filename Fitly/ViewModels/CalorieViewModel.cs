using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AudioToolbox;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Fitly.API;
using Fitly.Models;

namespace Fitly.ViewModels
{
    public partial class CalorieViewModel : ObservableObject
    {

        private readonly string url = "https://bgs.jedlik.eu/hm/backend/public/api/users";

        [ObservableProperty]
        User selectedUser;

        ObservableCollection<Meal> meals;

        [RelayCommand]
        async Task Appearing()
        {
            string? isLoginSet = SecureStorage.Default.GetAsync("LoginToken").Result;
            string? userID = SecureStorage.Default.GetAsync("UserId").Result;

            meals = await GetData.GetMeals(url);


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
        async Task CalculateCalorie()
        {
            if (SelectedUser.weight == null || SelectedUser.height == null || SelectedUser.lose_or_gain == null)
            {
                Shell.Current.DisplayAlert("Hiányos adatok", "kérlek töltsd fel az adataid", "Ok");
            }

            
        }

    }
}
