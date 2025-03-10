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
                Email = Email,
                Password = Password
            };

            string jsonData = JsonSerializer.Serialize(requestData);
            await Shell.Current.DisplayAlert("Elküldött JSON", jsonData, "OK");

            var response = await HTTPRequest<LoginUserResponse>.Post(url, requestData);

            if(response != null)
            {
                if(response.Token != null)
                {
                    await Shell.Current.DisplayAlert("Sikeres bejelentkezés!", "A bejelentkezés sikeres volt!", "Mégse");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Hiba", "Hiba", "Mégse");
                }
            }
            else
            {
                await Shell.Current.DisplayAlert("Hiba", "Kérlek add meg az adataid!", "ok");
            }

        }
    }
}
