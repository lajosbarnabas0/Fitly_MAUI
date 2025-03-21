﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Fitly.ViewModels
{
    public partial class LetsBeginViewModel : ObservableObject
    {
        [RelayCommand]
        async Task LoginClicked()
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }

        [RelayCommand]
        async Task RegistrationButtonClicked()
        {
            await Shell.Current.GoToAsync("//RegisterPage");
        }
    }
}
