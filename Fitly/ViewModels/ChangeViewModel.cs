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
    public partial class ChangeViewModel : ObservableObject
    {
        [RelayCommand]
        async Task NavigateToCalorie()
        {
            try
		    {
			    Shell.Current.GoToAsync(nameof(CaloriePage));
		    }
		    catch (Exception ex)
		    {
                Console.WriteLine($"Navigációs hiba: {ex.Message}");
                Console.WriteLine($"Részletek: {ex.StackTrace}");
            }
        }
        
        [RelayCommand]
        async Task NavigateToWorkout()
        {
            try
		    {
                Shell.Current.GoToAsync(nameof(WorkoutsPage));
		    }
		    catch (Exception ex)
		    {
                Console.WriteLine($"Navigációs hiba: {ex.Message}");
                Console.WriteLine($"Részletek: {ex.StackTrace}");
            }
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
