using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Fitly.API;
using Fitly.Models;

namespace Fitly.ViewModels
{
    public partial class PostListViewModel : ObservableObject
    {
        public ObservableCollection<Post> Posts { get; set; } = new ObservableCollection<Post>();

        [RelayCommand]
        async Task Appearing()
        {
            try
            {
                string apiUrl = "https://bgs.jedlik.eu/hm/backend/public/api/posts"; 
                var postsFromApi = await HTTPRequest<List<Post>>.Get(apiUrl);

                if (postsFromApi != null)
                {
                    Posts.Clear();
                    foreach (var post in postsFromApi)
                    {
                        Posts.Add(post);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hiba a postok letöltésekor: {ex.Message}");
            }
        }

        
    }
}
