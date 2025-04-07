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
        //[
        //    "https://www.kreactivity.hu/img/23190/G00176/264x264,r/Kis-tigris-festes-es-gyemantszemes-kreativ-hibrid-kep.webp?time=1715340732",
        //    "https://macska-nevek.hu/wp-content/uploads/2024/04/1-1024x576.jpg"
        //];
        public RecipeDetailViewModel()
        {
        }

        partial void OnSelectedRecipeChanged(Recipe value)
        {
            if(SelectedRecipe.Image_paths != null)
            {
                foreach (var item in SelectedRecipe.Image_paths)
                {
                    Image_paths.Add($"https://bgs.jedlik.eu/hm/backend/public/storage/" + item);
                }
            }
        }


        [RelayCommand]
        async Task Appearing()
        {
            string url = "https://bgs.jedlik.eu/hm/backend/public/api/users";
            int userID = SelectedRecipe.user_id;
            try
            {
                User? user = await GetData.GetUserById($"{url}/{userID}");
                if (user != null)
                {
                    UserName = user.Name;
                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                return;
            }
        }
    }
}
