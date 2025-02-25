using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Fitly.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        [RelayCommand]
        async Task IfNoAccountRegistered()
        {
            await Shell.Current.GoToAsync("//RegisterPage");
        }
    }
}
