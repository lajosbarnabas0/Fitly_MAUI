using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Fitly.Pages;

namespace Fitly.ViewModels
{
    public partial class WorkoutViewModel : ObservableObject
    {

        [RelayCommand]
        async Task NavigateToStepCounter()
        {
            await Shell.Current.GoToAsync(nameof(StepCounterPage));
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
