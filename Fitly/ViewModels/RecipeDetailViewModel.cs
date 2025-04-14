using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Fitly.API;
using Fitly.Models;

namespace Fitly.ViewModels
{
    [QueryProperty(nameof(SelectedRecipe), "Recipe")]

    public partial class RecipeDetailViewModel : ObservableObject
    {
        [ObservableProperty]
        Recipe selectedRecipe;

        [ObservableProperty]
        string userName;
        
        public RecipeDetailViewModel()
        {
        }

        [ObservableProperty]
        bool swipeEnabled = true;


        [RelayCommand]
        async Task Appearing()
        {
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;

            if (!(accessType == NetworkAccess.Internet))
            {
                await Shell.Current.DisplayAlert("Hiba", "Kérjük csatlakozzon az internethez!", "Ok");
            }

            if (SelectedRecipe.image_urls == null)
            {
                await Shell.Current.DisplayAlert("Hiba", "Hiba történt a recept betöltésekor!", "Ok");
                return;
            }

            try
            {

                if(SelectedRecipe.image_urls.Length > 1)
                {
                    SwipeEnabled = true;
                }
                else
                {
                    SwipeEnabled = false;
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Hiba", "Hiba történt!", "Ok");
            }
        }

        

    }
}
