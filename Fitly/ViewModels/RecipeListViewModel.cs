﻿using System;
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
    public partial class RecipeListViewModel : ObservableObject
    {
        public ObservableCollection<Recipe> Recipes { get; set; } = new ObservableCollection<Recipe>();

        [RelayCommand]
        async Task Appearing()
        {
            try
            {
                string apiUrl = "https://bgs.jedlik.eu/hm/backend/public/api/recipes";
                var recipesFromApi = await HTTPRequest<List<Recipe>>.Get(apiUrl);

                if (recipesFromApi != null)
                {
                    Recipes.Clear();
                    foreach (var recipe in recipesFromApi)
                    {
                        Recipes.Add(recipe);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hiba a receptek letöltésekor: {ex.Message}");
            }
        }

    }
}
