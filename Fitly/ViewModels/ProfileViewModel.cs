﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public partial class ProfileViewModel : ObservableObject, INotifyPropertyChanged
    {

        [ObservableProperty]
        bool isReadOnly = true;

        [ObservableProperty]
        bool isPickerEnabled = false;

        [ObservableProperty]
        User selectedUser;

        [ObservableProperty]
        User originalUser;

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
                        SelectedUser = user;
                        OriginalUser = user;

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
        async Task Logout()
        {
            SecureStorage.RemoveAll();
            await Shell.Current.GoToAsync("//MainPage");
        }

        [RelayCommand]

        async Task editButton()
        {
            IsReadOnly = false;
            IsPickerEnabled = true;
            OriginalUser = JsonSerializer.Deserialize<User>(JsonSerializer.Serialize(SelectedUser));
        }

        [RelayCommand]
        async Task cancelButton()
        {
            IsReadOnly = true;
            IsPickerEnabled = false;
            SelectedUser.weight = OriginalUser.weight;
            SelectedUser.height = OriginalUser.height;
            SelectedUser.lose_or_gain = OriginalUser.lose_or_gain;
            SelectedUser.goal_weight = OriginalUser.goal_weight;

            OnPropertyChanged(nameof(SelectedUser));
        }

        [RelayCommand]
        async Task saveButton()
        {
            if (IsUserDataChanged())
            {
                string url = "https://bgs.jedlik.eu/hm/backend/public/api/users/profile";
                var requestData = new User
                {
                    weight = SelectedUser.weight,
                    height = SelectedUser.height,
                    lose_or_gain = SelectedUser.lose_or_gain,
                    goal_weight = SelectedUser.goal_weight
                };

                var response = await GetData.UpdateProfile(url, requestData);

                if (response != null)
                {
                    if (response != null)
                    {
                        Console.WriteLine(response.message);
                    }
                    else
                    {
                        await Shell.Current.DisplayAlert("Hiba", "Sikertelen küldés", "Ok");
                    }
                }
                else
                {
                    await Shell.Current.DisplayAlert("Hiba", "Hiba történt!", "Ok");
                }
                IsReadOnly = true;
                IsPickerEnabled = false;
            }
            else
            {
                await Shell.Current.DisplayAlert("Hiba", "Nem történt változtatás az adatokban!", "Ok");
            }
        }

        private bool IsUserDataChanged()
        {
            return SelectedUser.weight != OriginalUser.weight || SelectedUser.height != OriginalUser.height || SelectedUser.lose_or_gain != OriginalUser.lose_or_gain || SelectedUser.goal_weight != OriginalUser.goal_weight;
        }
    }
}
