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
        CommentRequest comment = new CommentRequest();

        [ObservableProperty]
        ObservableCollection<Comment> comments = new ObservableCollection<Comment>();

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
                    CommentEnabled = true;
                }

                if(commentsFromApi != null)
                {
                    Comments.Clear();
                    foreach(var comment in commentsFromApi)
                    {
                        Comments.Add(comment);
                    }
                }
                OnPropertyChanged(nameof(Comments));
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
                content = Comment.content,
                user_id = SelectedPost.user_id
            };

            var response = await SendData.SendComment(url, requestData);

            if (response != null)
            {
                if (response != null)
                {
                    await Shell.Current.DisplayAlert("Információ", "Sikeres komment küldés!", "Ok");
                    OnPropertyChanged(nameof(Comments));
                    Comment.content = "";
                }
                else
                {
                    await Shell.Current.DisplayAlert("Hiba", "Sikertelen küldés!", "Ok");
                    return;
                }
            }
            else
            {
                await Shell.Current.DisplayAlert("Hiba", "Hiba történt!", "Ok");
                return;
            }

        }
    }
}
