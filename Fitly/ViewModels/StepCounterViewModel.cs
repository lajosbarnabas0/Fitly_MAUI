using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Plugin.Maui.Pedometer;

namespace Fitly.ViewModels
{
    public partial class StepCounterViewModel : ObservableObject
    {

        private int _baselineSteps = 0;
        IPedometer pedometer;

        [ObservableProperty]
        int numberOfSteps;

        public StepCounterViewModel(IPedometer pedometer)
        {
            this.pedometer = pedometer;
        }

        [RelayCommand]
        public void StartCounting()
        {
            pedometer.ReadingChanged += (sender, reading) =>
            {
                NumberOfSteps = reading.NumberOfSteps - _baselineSteps;
            };

            pedometer.Start();
        }

        [RelayCommand]
        public void StopCounting()
        {
            pedometer.Stop();
        }

        [RelayCommand]
        public void ResetCounting()
        {
            _baselineSteps = NumberOfSteps + _baselineSteps; 
            NumberOfSteps = 0; 
        }

        [RelayCommand]
        async Task Appearing()
        {
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;

            if (!(accessType == NetworkAccess.Internet))
            {
                await Shell.Current.DisplayAlert("Hiba", "Kérjük csatlakozzon az internethez!", "Ok");
            }
        }

    }
}
