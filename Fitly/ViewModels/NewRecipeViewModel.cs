using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Fitly.API;
using Fitly.Models;
using Fitly.Pages;

namespace Fitly.ViewModels
{
    public partial class NewRecipeViewModel : ObservableObject
    {
        [ObservableProperty]
        Recipe newRecipe = new Recipe();

        public FileResult SelectedImageFile { get; private set; }

        [RelayCommand]
        public async Task AddImagesAsync()
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
                Console.WriteLine($"Hiba kép kiválasztásnál: {ex.Message}");
            }
        }

        [RelayCommand]
        async Task SaveRecipe()
        {
            try
            {
                if (SelectedImageFile == null)
                {
                    Console.WriteLine("Nincs kiválasztott fájl.");
                    return;
                }

                // Létrehozzuk a HttpClient példányt
                using var httpClient = new HttpClient();

                // Hozzáadjuk a szükséges headereket
                httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64)");
                var token = await SecureStorage.Default.GetAsync("LoginToken");
                if (!string.IsNullOrEmpty(token))
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }

                // Létrehozzuk a MultipartFormDataContent-et
                using var formData = new MultipartFormDataContent();

                // Fájl hozzáadása form-data-hoz
                var fileStream = await SelectedImageFile.OpenReadAsync();
                var streamContent = new StreamContent(fileStream);
                streamContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
                formData.Add(streamContent, "image_paths[]", SelectedImageFile.FileName);

                // Egyéb mezők hozzáadása
                formData.Add(new StringContent(NewRecipe.title), "title");
                formData.Add(new StringContent(NewRecipe.ingredients), "ingredients");
                formData.Add(new StringContent(NewRecipe.description), "description");
                formData.Add(new StringContent(NewRecipe.avg_time), "avg_time");

                // Küldés az API végpontra
                var url = "https://bgs.jedlik.eu/hm/backend/public/api/recipes";
                var response = await httpClient.PostAsync(url, formData);

                // Ellenőrizzük a választ
                if (response.IsSuccessStatusCode)
                {
                    Shell.Current?.DisplayAlert("Siker", "A recept mentése sikeres volt!", "Ok");
                    Console.WriteLine("Fájl sikeresen feltöltve!");
                }
                else
                {
                    Console.WriteLine($"Hiba történt: {response.StatusCode}");
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Hibaüzenet: {errorMessage}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hiba fájlfeltöltés közben: {ex.Message}");
            }

        }
    }
}
