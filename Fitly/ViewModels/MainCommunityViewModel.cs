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
    public partial class MainCommunityViewModel : ObservableObject
    {
        [RelayCommand]
        async Task GoToPosts()
        {
            await Shell.Current.GoToAsync(nameof(PostListPage));
        }

        [RelayCommand]
        async Task GoToRecipes()
        {
            await Shell.Current.GoToAsync(nameof(RecipeListPage));
        }
    }
}
