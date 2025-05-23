﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Fitly.API;
using Fitly.Models;
using Microsoft.Maui.Networking;


namespace Fitly.ViewModels
{

    public partial class LoginViewModel : ObservableObject
    {
        [ObservableProperty]
        string? email;

        [ObservableProperty]
        string? password;

        [RelayCommand]
        async Task Appearing()
        {
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;

            if (!(accessType == NetworkAccess.Internet))
            {
                await Shell.Current.DisplayAlert("Hiba", "Kérjük csatlakozzon az internethez!", "Ok");
            }
        }

        private void ResetFields()
        {
            Email = string.Empty;
            Password = string.Empty;
        }

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
                    await Shell.Current.DisplayAlert("Információ", "Sikeres bejelentkezés", "Ok");
                    await Shell.Current.GoToAsync("//ProfilePage");
                    ResetFields();
                }
                else
                {
                    await Shell.Current.DisplayAlert("Hiba", "Sikertelen bejelentkezés", "Ok");
                }
            }
            else
            {
                await Shell.Current.DisplayAlert("Hiba", "Hiba történt!", "Ok");
            }
        }

        [RelayCommand]
        public async Task IfNoAccountRegistered()
        {
            Shell.Current.GoToAsync("//RegisterPage");
        }


    }
}
