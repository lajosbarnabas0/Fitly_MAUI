using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Fitly.ViewModels
{
    public partial class RegisterViewModel : ObservableObject
    {
        [RelayCommand]
        async Task IfAlreadyHaveAccount()
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
