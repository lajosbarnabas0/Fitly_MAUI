using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Fitly.API;
using Fitly.Models;

namespace Fitly.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
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
        async Task LetsBeginButtonClicked()
        {
            string? isLoginSet = SecureStorage.Default.GetAsync("LoginToken").Result;

            if(isLoginSet != null)
            {
                await Shell.Current.GoToAsync("//ProfilePage");
            }
            else
            {
                await Shell.Current.GoToAsync("//LetsBeginPage");
            }

        }
    }
}
