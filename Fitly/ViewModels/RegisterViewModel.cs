using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Fitly.API;
using Fitly.Models;
using static System.Net.WebRequestMethods;

namespace Fitly.ViewModels
{
    public partial class RegisterViewModel : ObservableObject
    {
        [ObservableProperty]
        string name;

        [ObservableProperty]
        string email;

        [ObservableProperty]
        string password;

        [ObservableProperty]
        string password_confirmation;

        [ObservableProperty]
        string birthday;

        [ObservableProperty]
        string gender;

        [RelayCommand]
        async Task IfAlreadyHaveAccount()
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }

        [RelayCommand]
        public async Task Register()
        {
            string url = "https://bgs.jedlik.eu/hm/backend/public/api/register";
            var requestData = new RegisterUserRequest
            {
                name = Name,
                email = Email,
                password = Password,
                password_confirmation = Password_confirmation,
                birthday = Birthday,
                gender = Gender
            };

            var response = await AuthData.RegisterUser(url, requestData);

            if (response != null)
            {
                if (response != null)
                {
                    await Shell.Current.GoToAsync("//ProfilePage");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Hiba", "Sikertelen regisztráció", "Ok");
                }
            }
            else
            {
                await Shell.Current.DisplayAlert("Hiba", "Kérlek add meg az adataid!", "Ok");
            }
        }
    }
}
