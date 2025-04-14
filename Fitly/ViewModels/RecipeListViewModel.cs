using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Fitly.API;
using Fitly.Models;
using Fitly.Pages;

namespace Fitly.ViewModels
{
    public partial class RecipeListViewModel : ObservableObject
    {
        public ObservableCollection<Recipe> Recipes { get; set; } = new ObservableCollection<Recipe>();

        [ObservableProperty]
        Recipe selectedRecipe;

        [ObservableProperty]
        bool isVisible = false;

        [RelayCommand]
        async Task AddNewRecipe()
        {
            await Shell.Current.GoToAsync(nameof(NewRecipePage));
        }

        [RelayCommand]
        async Task NavigateToOwnRecipes()
        {
            await Shell.Current.GoToAsync(nameof(OwnRecipeListPage));
        }

        [RelayCommand]
        async Task Appearing()
        {
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;

            if (!(accessType == NetworkAccess.Internet))
            {
                await Shell.Current.DisplayAlert("Hiba", "Kérjük csatlakozzon az internethez!", "Ok");
            }

            try
            {
                string? isLoginSet = await SecureStorage.Default.GetAsync("LoginToken");
                string apiUrl = "https://bgs.jedlik.eu/hm/backend/public/api/recipes";
                var recipesFromApi = await HTTPRequest<ObservableCollection<Recipe>>.Get(apiUrl);

                if (recipesFromApi != null)
                {
                    Recipes.Clear();
                    foreach (var recipe in recipesFromApi)
                    {
                        Recipes.Add(recipe);
                    }
                }

                if (isLoginSet != null)
                {
                    IsVisible = true;
                }
                else
                {
                    IsVisible = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hiba a receptek letöltésekor: {ex.Message}");
            }
        }

        [RelayCommand]
        async Task NavigateToRecipeDetail(Recipe recipe)
        {
            if (recipe == null)
            {
                return;
            }

            await Shell.Current.GoToAsync(nameof(RecipeDetailPage) , new Dictionary<string, object>
            {
                { "Recipe", recipe }
            });
        }
    }
}
