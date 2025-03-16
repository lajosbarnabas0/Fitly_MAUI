using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Fitly.ViewModels
{
    public partial class ProfileViewModel : ObservableObject
    {
        [RelayCommand]
        async Task Logout()
        {
            SecureStorage.RemoveAll();
            await Shell.Current.GoToAsync("//MainPage");
        }
    }
}
