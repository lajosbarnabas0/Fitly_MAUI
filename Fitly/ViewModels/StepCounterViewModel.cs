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
        readonly IPedometer pedometer;

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
                NumberOfSteps = reading.NumberOfSteps;
            };

            pedometer.Start();
        }

        [RelayCommand]
        public void StopCounting()
        {
            pedometer.Stop();
            NumberOfSteps = 0;
        }
    }
}
