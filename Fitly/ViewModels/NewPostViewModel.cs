using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Fitly.API;
using Fitly.Models;

namespace Fitly.ViewModels
{
    public partial class NewPostViewModel: ObservableObject
    {
        [ObservableProperty]
        Post newPost = new();

        public List<FileResult> SelectedImageFiles { get; private set; } = new();

        [RelayCommand]
        public async Task AddImages()
        {
            try
            {
                var files = await FilePicker.PickMultipleAsync(new PickOptions
                {
                    FileTypes = FilePickerFileType.Images,
                    PickerTitle = "Válassz képeket a recepthez"
                });

                if (files is null || !files.Any())
                    return;

                SelectedImageFiles = files.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hiba kép kiválasztásnál: {ex.Message}");
            }
        }

        [RelayCommand]
        async Task SavePost()
        {
            string url = "https://bgs.jedlik.eu/hm/backend/public/api/users/posts";
            var requestData = new PostRequest
            {
                Title = NewPost.Title,
                Content = NewPost.Content,
            };

            var response = await SendData.SendPost(url, requestData);

            if (response != null)
            {
                if (response != null)
                {
                    Console.WriteLine(response);
                }
                else
                {
                    await Shell.Current.DisplayAlert("Hiba", "Sikertelen küldés", "Ok");
                }
            }
            else
            {
                await Shell.Current.DisplayAlert("Hiba", "Hiba történt!", "Ok");
            }
        }
    }
}
