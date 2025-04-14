using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Fitly.API;
using Fitly.Models;

namespace Fitly.ViewModels
{
    public partial class OwnRecipeListViewModel : ObservableObject
    {
        public ObservableCollection<Recipe> Recipes { get; set; } = new ObservableCollection<Recipe>();

        [ObservableProperty]
        public ObservableCollection<Recipe> usersRecipes = new ObservableCollection<Recipe>();

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

            try
            {
                string? isLoginSet = await SecureStorage.Default.GetAsync("LoginToken");
                string? userID = await SecureStorage.Default.GetAsync("UserId");

                string apiUrl = "https://bgs.jedlik.eu/hm/backend/public/api/recipes";
                var recipesFromApi = await HTTPRequest<ObservableCollection<Recipe>>.Get(apiUrl);

                if (recipesFromApi != null)
                {
                    Recipes.Clear();
                    foreach (var recipe in recipesFromApi)
                    {
                        Recipes.Add(recipe);
                    }

                    if (int.TryParse(userID, out int userIdInt)) // Megpróbáljuk konvertálni a userID-t int-re
                    {
                        UsersRecipes.Clear(); // Töröljük az usersPosts gyűjteményt
                        foreach (var post in Recipes.Where(x => x.user_id == userIdInt)) // Szűrés a konvertált értékkel
                        {
                            UsersRecipes.Add(post); // Csak a feltételnek megfelelő elemeket adjuk hozzá
                        }
                    }
                    else
                    {
                        Console.WriteLine("Hiba: Az userID nem konvertálható int típusra.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hiba a postok letöltésekor: {ex.Message}");
            }
        }

        [RelayCommand]
        async Task DeleteRecipe(Recipe recipe)
        {
            bool answer = await Shell.Current.DisplayAlert("Megerősítés", "Biztosan törölni akarja a receptet?", "Igen", "Nem");
            string url = $"https://bgs.jedlik.eu/hm/backend/public/api/recipes/{recipe.id}";

            if (recipe != null && UsersRecipes.Contains(recipe) && answer)
            {
                UsersRecipes.Remove(recipe);
                var message = SendData.DeletePost(url);

                if (message != null)
                {
                    await Shell.Current.DisplayAlert("Információ", "Sikeres törlés", "Ok");
                    return;
                }
            }
        }

    }
}
