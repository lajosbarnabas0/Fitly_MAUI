using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Fitly.Models;

namespace Fitly.ViewModels
{
    public partial class RecipeDetailViewModel : ObservableObject
    {
        public RecipeDetailViewModel()
        {

        }

        [RelayCommand]
        async Task BackButton()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
