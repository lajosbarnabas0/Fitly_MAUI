using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Fitly.Models;

namespace Fitly.ViewModels
{
    public partial class NewRecipeViewModel : ObservableObject
    {
        [ObservableProperty]
        Recipe newRecipe;


    }
}
