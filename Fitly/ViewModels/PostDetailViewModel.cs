using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Fitly.API;
using Fitly.Models;

namespace Fitly.ViewModels
{
    [QueryProperty(nameof(SelectedPost), "Post")]
    public partial class PostDetailViewModel : ObservableObject
    {
        [ObservableProperty]
        bool commentEnabled = false;

        [ObservableProperty]
        CommentRequest comment;

        [ObservableProperty]
        List<Comment> comments = new List<Comment>();

        [ObservableProperty]
        public Post selectedPost;

        public ObservableCollection<string> Image_paths { get; set; }
        //[
        //    "https://www.kreactivity.hu/img/23190/G00176/264x264,r/Kis-tigris-festes-es-gyemantszemes-kreativ-hibrid-kep.webp?time=1715340732",
        //    "https://macska-nevek.hu/wp-content/uploads/2024/04/1-1024x576.jpg"
        //];


        [RelayCommand]
        async Task Appearing()
        {
            try
            {
                string? isLoginSet = SecureStorage.Default.GetAsync("LoginToken").Result;
                string url = $"https://bgs.jedlik.eu/hm/backend/public/api/posts/{SelectedPost.id}/comments";
                var commentsFromApi = await GetData.GetComment(url);

                if (isLoginSet != null)
                {
                    commentEnabled = true;

                }
                else 
                {
                    commentEnabled = false;
                }

                if(commentsFromApi != null)
                {
                    Comments.Clear();
                    foreach(var comment in commentsFromApi)
                    {
                        Comments.Add(comment);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                return;
            }
        }

        [RelayCommand]
        async Task SendComment()
        {
            string url = $"https://bgs.jedlik.eu/hm/backend/public/api/posts/{SelectedPost.id}/comments";
            var requestData = new CommentRequest
            {
                content = Selected
            };

            var response = await AuthData.LoginUser(url, requestData);

            if (response != null)
            {
                if (response.token != null)
                {
                    await SecureStorage.Default.SetAsync("LoginToken", response.token);
                    await SecureStorage.Default.SetAsync("UserId", response.user.id.ToString());
                    await Shell.Current.GoToAsync("//ProfilePage");

                }
                else
                {
                    await Shell.Current.DisplayAlert("Hiba", "Sikertelen bejelentkezés", "Ok");
                }
            }
            else
            {
                await Shell.Current.DisplayAlert("Hiba", "Kérlek add meg az adataid!", "Ok");
            }

        }
    }
}
