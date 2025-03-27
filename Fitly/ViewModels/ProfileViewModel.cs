using System;
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
        bool isPickerEnabled = true;

        [ObservableProperty]
        User selectedUser;

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
            IsPickerEnabled = false;
        }

        [RelayCommand]
        async Task cancelButton()
        {
            IsReadOnly = true;
            IsPickerEnabled = true;
        }

        [RelayCommand]
        async Task saveButton()
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
                    Console.WriteLine(response.name);
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
    }
}
