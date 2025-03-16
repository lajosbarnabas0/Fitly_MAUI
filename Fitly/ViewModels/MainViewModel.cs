using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Fitly.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        [RelayCommand]
        async Task LetsBeginButtonClicked()
        {
            string? isLoginSet = SecureStorage.Default.GetAsync("LoginToken").Result;

            if(isLoginSet != null)
            {
                await Shell.Current.GoToAsync("//ProfilePage");
            }
            else
            {
                await Shell.Current.GoToAsync("//LetsBeginPage");
            }

        }
    }
}
