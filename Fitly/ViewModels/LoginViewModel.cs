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

        [ObservableProperty]
        string? responseMessage;

        [RelayCommand]
        public async Task Login()
        {
            string url = "https://bgs.jedlik.eu/hm/backend/public/api/login";
            var requestData = new LoginUserRequest
            {
                email = Email,
                password = Password
            };

            var response = await HTTPRequest<LoginUserResponse>.Post(url, requestData);

            if(response != null)
            {
                if(response.token != null)
                {
                    ResponseMessage = "Sikeres bejelentkezés!";
                    await SecureStorage.Default.SetAsync("LoginToken", response.token);
                }
                else
                {
                    ResponseMessage = "Sikertelen bejelentkezés!";
                }
            }
            else
            {
                await Shell.Current.DisplayAlert("Hiba", "Kérlek add meg az adataid!", "ok");
            }
        }
    }
}
