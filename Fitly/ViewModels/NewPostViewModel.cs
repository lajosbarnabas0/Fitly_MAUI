using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Fitly.API;
using Fitly.Models;
using Fitly.Pages;

namespace Fitly.ViewModels
{
    public partial class NewPostViewModel: ObservableObject
    {
        [ObservableProperty]
        Post newPost = new();

        public FileResult SelectedImageFile { get; private set; }

        [RelayCommand]
        public async Task AddImages()
        {
            try
            {
                var result = await MediaPicker.PickPhotoAsync();
                if (result != null)
                {
                    SelectedImageFile = result;
                    Shell.Current?.DisplayAlert("Feltöltés", "A kép feltöltése sikeres!", "Ok");
                }
            }
            catch (Exception ex)
            {
                Shell.Current?.DisplayAlert("Feltöltés", "A kép feltöltése sikertelen!", "Ok");
                Console.WriteLine($"Hiba kép kiválasztásnál: {ex.Message}");
                return;
            }
        }
         
        [RelayCommand]
        async Task SavePost()
        {
            try
            {
                using var httpClient = new HttpClient();

                httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64)");
                var token = await SecureStorage.Default.GetAsync("LoginToken");
                if (!string.IsNullOrEmpty(token))
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }

                using var formData = new MultipartFormDataContent();

                // Fájl hozzáadása form-data-hoz
                if(SelectedImageFile != null)
                {
                    var fileStream = await SelectedImageFile.OpenReadAsync();
                    var streamContent = new StreamContent(fileStream);
                    streamContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
                    formData.Add(streamContent, "image", SelectedImageFile.FileName);
                }

                // Egyéb mezők hozzáadása
                formData.Add(new StringContent(NewPost.Title), "title");
                formData.Add(new StringContent(NewPost.Content), "content");

                var url = "https://bgs.jedlik.eu/hm/backend/public/api/posts";
                var response = await httpClient.PostAsync(url, formData);

                if (response.IsSuccessStatusCode)
                {
                    Shell.Current?.DisplayAlert("Siker", "A bejegyzés mentése sikeres volt!", "Ok");
                    Shell.Current.GoToAsync(nameof(PostListPage));
                    Console.WriteLine("Fájl sikeresen feltöltve!");
                }
                else
                {
                    Console.WriteLine($"Hiba történt: {response.StatusCode}");
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    Shell.Current?.DisplayAlert("Hiba", "Hiba történt!", "Ok");
                    Console.WriteLine($"Hibaüzenet: {errorMessage}");
                }
            }
            catch (Exception ex)
            {
                Shell.Current?.DisplayAlert("Hiba", "Hiba történt!", "Ok");
                Console.WriteLine($"Hiba fájlfeltöltés közben: {ex.Message}");
                return;
            }

        }

        [RelayCommand]
        async Task Appearing()
        {
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;

            if (!(accessType == NetworkAccess.Internet))
            {
                await Shell.Current.DisplayAlert("Hiba", "Kérjük csatlakozzon az internethez!", "Ok");
            }
        }
    }
}
