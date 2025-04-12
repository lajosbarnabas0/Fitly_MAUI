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
    public partial class OwnPostListViewModel : ObservableObject
    {
        public ObservableCollection<Post> Posts { get; set; } = new ObservableCollection<Post>();

        [ObservableProperty]
        public ObservableCollection<Post> usersPosts = new ObservableCollection<Post>();

        [RelayCommand]
        async Task Appearing()
        {
            try
            {
                string? isLoginSet = await SecureStorage.Default.GetAsync("LoginToken");
                string? userID = await SecureStorage.Default.GetAsync("UserId");

                string apiUrl = "https://bgs.jedlik.eu/hm/backend/public/api/posts";
                var postsFromApi = await HTTPRequest<ObservableCollection<Post>>.Get(apiUrl);

                if (postsFromApi != null)
                {
                    Posts.Clear();
                    foreach (var post in postsFromApi)
                    {
                        Posts.Add(post);
                    }

                    UsersPosts.Clear();
                    if (int.TryParse(userID, out int userIdInt)) // Megpróbáljuk konvertálni a userID-t int-re
                    {
                        UsersPosts.Clear(); // Töröljük az usersPosts gyűjteményt
                        foreach (var post in Posts.Where(x => x.user_id == userIdInt)) // Szűrés a konvertált értékkel
                        {
                            UsersPosts.Add(post); // Csak a feltételnek megfelelő elemeket adjuk hozzá
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
        async Task DeletePost(Post post)
        {
            bool answer = await Shell.Current.DisplayAlert("Megerősítés", "Biztosan törölni akarja a bejegyzést?", "Igen", "Nem");
            string url = $"https://bgs.jedlik.eu/hm/backend/public/api/posts/{post.id}";

            if (post != null && UsersPosts.Contains(post) && answer)
            {
                UsersPosts.Remove(post);
                var message = SendData.DeletePost(url);

                if(message != null)
                {
                    await Shell.Current.DisplayAlert("Információ", "Sikeres törlés", "Ok");
                    return;
                }
            }
        }
    }
}
