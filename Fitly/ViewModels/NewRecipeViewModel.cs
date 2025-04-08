using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Fitly.API;
using Fitly.Models;
using Fitly.Pages;

namespace Fitly.ViewModels
{
    public partial class NewRecipeViewModel : ObservableObject
    {
        [ObservableProperty]
        Recipe newRecipe;

        [RelayCommand]
        public async Task AddImagesAsync()
        {
            try
            {
                var result = await FilePicker.PickMultipleAsync(new PickOptions
                {
                    PickerTitle = "Válassz képeket",
                    FileTypes = FilePickerFileType.Images // Csak képek engedélyezése
                });

                if (result != null)
                {
                    var imagePaths = result.Select(file => file.FullPath).ToString();
                    NewRecipe.image_paths = imagePaths; // A kiválasztott képek útvonalainak mentése
                }
            }
            catch (Exception ex)
            {
                // Hibakezelés
                Console.WriteLine($"Hiba történt: {ex.Message}");
            }
        }

        [RelayCommand]
        async Task SaveRecipe()
        {
            string url = "https://bgs.jedlik.eu/hm/backend/public/api/recipes";
            var requestData = new Recipe
            {
                title = NewRecipe.title,
                avg_time = NewRecipe.avg_time,
                image_paths = NewRecipe.image_paths,
                description = NewRecipe.description,
                ingredients = NewRecipe.ingredients

            };

            var response = await SendData.SendRecipe(url, requestData);

            if (response != null)
            {
                if (response != null)
                {
                    await Shell.Current.GoToAsync(nameof(RecipeListPage));
                    await Shell.Current.DisplayAlert("Információ", "Recept sikeresen elmentve!", "Ok");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Hiba", "Sikertelen regisztráció", "Ok");
                }
            }
            else
            {
                await Shell.Current.DisplayAlert("Hiba", "Kérlek add meg az adataid!", "Ok");
            }
        }
    }
}
