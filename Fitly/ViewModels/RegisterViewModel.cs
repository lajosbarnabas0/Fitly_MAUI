using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Fitly.API;
using Fitly.Helper;
using Fitly.Models;
using Fitly.Pages;
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

        private string ConvertGenderToServerFormat(string gender)
        {
            return gender switch
            {
                "Férfi" => "male",
                "Nő" => "female",
            };
        }

        private void ResetFields()
        {
            Name = string.Empty;
            Email = string.Empty;
            Password = string.Empty;
            Password_confirmation = string.Empty;
            Birthday = null; 
            Gender = null; 
        }

        [RelayCommand]
        public async Task Register()
        {
            try
            {
                if (!ValidateFields())
                {
                    await Shell.Current.DisplayAlert("Hiba", "Kérjük, töltse ki az összes mezőt.", "Ok");
                    return;
                }

                if (!ValidatePassword(Password))
                {
                    await Shell.Current.DisplayAlert("Hiba", "A jelszónak legalább 8 karakter hosszúnak kell lennie, tartalmaznia kell egy nagybetűt és egy számot.", "Ok");
                    return;
                }

                if(!PasswordsMatch(Password, Password_confirmation))
                {
                    await Shell.Current.DisplayAlert("Hiba", "A két jelszó nem egyezik!", "Ok");
                    return;
                }

                // Mezők validációja

                string url = "https://bgs.jedlik.eu/hm/backend/public/api/register";
                var requestData = new RegisterUserRequest
                {
                    name = Name,
                    email = Email,
                    password = Password,
                    password_confirmation = Password_confirmation,
                    birthday = Birthday,
                    gender = ConvertGenderToServerFormat(Gender)
                };

                // Jelszó validáció

                // API hívás
                var response = await AuthData.RegisterUser(url, requestData);

                // Sikeres válasz kezelése
                if (response != null )
                {
                    await Shell.Current.DisplayAlert("Információ", "Sikeres regisztráció!", "Ok");
                    await Shell.Current.GoToAsync("//LoginPage");
                    ResetFields();
                }
                else
                {
                    await Shell.Current.DisplayAlert("Hiba", $"Sikertelen regisztráció", "Ok");
                }
            }
            catch (Exception ex)
            {
                // Hibakezelés: váratlan hiba esetén jeleníts meg üzenetet
                await Shell.Current.DisplayAlert("Hiba", "Váratlan hiba történt a regisztráció során. Kérjük, próbálja újra később.", "Ok");
            }
        }

        private bool ValidatePassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return false;

            if (password.Length < 8)
                return false;

            if (!password.Any(char.IsUpper))
                return false;

            if (!password.Any(char.IsDigit))
                return false;

            return true;
        }

        private bool ValidateFields()
        {
            if (string.IsNullOrWhiteSpace(Name) ||
                string.IsNullOrWhiteSpace(Email) ||
                string.IsNullOrWhiteSpace(Password) ||
                string.IsNullOrWhiteSpace(Password_confirmation) ||
                string.IsNullOrWhiteSpace(Birthday) ||
                string.IsNullOrWhiteSpace(Gender))
            {
                return false;
            }
            return true;
        }

        private bool PasswordsMatch(string p1, string p2)
        {
            if(p1 != p2 || p1.Length != p2.Length)
                return false;
            return true;
        }

        [RelayCommand]
        async Task Appearing()
        {
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;

            if (accessType == NetworkAccess.Internet)
            {
                return;
            }
            else
            {
                await Shell.Current.DisplayAlert("Hiba", "Kérjük csatlakozzon az internethez!", "Ok");
                return;
            }
        }
    }
}
