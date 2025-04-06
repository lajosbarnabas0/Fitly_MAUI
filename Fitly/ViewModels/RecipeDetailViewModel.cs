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
        public ObservableCollection<string> Image_paths { get; set; } = new();
        public RecipeDetailViewModel()
        {
        }

        partial void OnSelectedRecipeChanged(Recipe value)
        {
            foreach (var img in Image_paths)
            {
                Console.WriteLine($"Kép útvonal: {img}");
            }
            // Ez fog lefutni automatikusan, ha jön adat navigation-ból
            if (value != null)
            {
                Image_paths = new ObservableCollection<string>(value.Image_paths);
                OnPropertyChanged(nameof(Image_paths));
            }
        }


        //[RelayCommand]
        //async Task Appearing()
        //{
        //    string url = "https://bgs.jedlik.eu/hm/backend/public/api/users";
        //    int userID = SelectedRecipe.user_id;
        //    try
        //    {
        //        User? user = await GetData.GetUserById($"{url}/{userID}");
        //        if (user != null)
        //        {
        //            UserName = user.Name;
        //        }
        //        else
        //        {
        //            return;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return;
        //    }
        //}
    }
}
