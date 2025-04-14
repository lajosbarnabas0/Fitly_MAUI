using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices.ObjectiveC;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Fitly.API;
using Fitly.Models;
using Fitly.Pages;

namespace Fitly.ViewModels
{
    public partial class PostListViewModel : ObservableObject
    {
        [ObservableProperty]
        public Post selectedPost;

        [ObservableProperty]
        bool isVisible = false;

        public ObservableCollection<Post> Posts { get; set; } = new ObservableCollection<Post>();

        [RelayCommand]
        async Task AddNewPost()
        {
            await Shell.Current.GoToAsync(nameof(NewPostPage));
        }

        [RelayCommand]
        async Task NavigateToOwnPosts()
        {
            await Shell.Current.GoToAsync(nameof(OwnPostListPage));
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
                string? isLoginSet = SecureStorage.Default.GetAsync("LoginToken").Result;
                string apiUrl = "https://bgs.jedlik.eu/hm/backend/public/api/posts"; 
                var postsFromApi = await HTTPRequest<ObservableCollection<Post>>.Get(apiUrl);

                if (postsFromApi != null)
                {
                    Posts.Clear();
                    foreach (var post in postsFromApi)
                    {
                        Posts.Add(post);
                    }
                }

                if(isLoginSet != null)
                {
                    IsVisible = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hiba a postok letöltésekor: {ex.Message}");
            }

            
        }

        [RelayCommand]
        private async Task NavigateToPostDetail(Post post)
        {

            if (post == null)
            {
                return;
            }

            await Shell.Current.GoToAsync("PostDetailPage", new Dictionary<string, object>
            {
                { "Post", post }
            });
        }
    }
}
