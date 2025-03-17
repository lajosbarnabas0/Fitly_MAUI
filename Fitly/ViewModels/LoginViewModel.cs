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

    public partial class LoginViewModel : ObservableObject
    {
        [ObservableProperty]
        string? email;

        [ObservableProperty]
        string? password;

        [RelayCommand]
        public async Task Login()
        {
            string url = "https://bgs.jedlik.eu/hm/backend/public/api/login";
            var requestData = new LoginUserRequest
            {
                email = Email,
                password = Password
            };

            var response = await AuthData.LoginUser(url, requestData);

            if(response != null)
            {
                if (response.token != null)
                {
                    await SecureStorage.Default.SetAsync("LoginToken", response.token);
                    await SecureStorage.Default.SetAsync("UserId", response.user.id.ToString());
                    await Shell.Current.GoToAsync("//ProfilePage");

                }
                else
                {
                    await Shell.Current.DisplayAlert("Hiba", "Sikertelen bejelentkezés", "Ok");
                }
            }
            else
            {
                await Shell.Current.DisplayAlert("Hiba", "Kérlek add meg az adataid!", "Ok");
            }
        }

        [RelayCommand]
        public async Task IfNoAccountRegistered()
        {
            Shell.Current.GoToAsync("//RegisterPage");
        }
    }
}
