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
        bool commentEnabled;

        [ObservableProperty]
        string comments;

        [ObservableProperty]
        public Post selectedPost;

        public ObservableCollection<string> Image_paths { get; set; }
        //[
        //    "https://www.kreactivity.hu/img/23190/G00176/264x264,r/Kis-tigris-festes-es-gyemantszemes-kreativ-hibrid-kep.webp?time=1715340732",
        //    "https://macska-nevek.hu/wp-content/uploads/2024/04/1-1024x576.jpg"
        //];

        partial void OnSelectedPostChanged(Post value)
        {
            if (SelectedPost.image_path != null)
            {
                foreach (var item in SelectedPost.image_path)
                {
                    Image_paths.Add($"https://bgs.jedlik.eu/hm/backend/public/storage/" + item);
                }
            }
        }


        [RelayCommand]
        async Task Appearing()
        {
            string? isLoginSet = SecureStorage.Default.GetAsync("LoginToken").Result;

            if (isLoginSet != null)
            {
                try
                {
                    commentEnabled = true;
                }
                catch (Exception ex)
                {
                    return;
                }
            }
            else 
            {
                commentEnabled = false;
            }
        }

        //[RelayCommand]
        //async Task SendComment()
        //{
        //    string url = $"https://bgs.jedlik.eu/hm/backend/public/api/posts/{SelectedPost.id}/comments";
        //    var requestData = new LoginUserRequest
        //    {
                
        //    };

        //    var response = await AuthData.LoginUser(url, requestData);

        //    if (response != null)
        //    {
        //        if (response.token != null)
        //        {
        //            await SecureStorage.Default.SetAsync("LoginToken", response.token);
        //            await SecureStorage.Default.SetAsync("UserId", response.user.id.ToString());
        //            await Shell.Current.GoToAsync("//ProfilePage");

        //        }
        //        else
        //        {
        //            await Shell.Current.DisplayAlert("Hiba", "Sikertelen bejelentkezés", "Ok");
        //        }
        //    }
        //    else
        //    {
        //        await Shell.Current.DisplayAlert("Hiba", "Kérlek add meg az adataid!", "Ok");
        //    }

        //}
    }
}
