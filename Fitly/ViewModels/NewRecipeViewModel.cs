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

        public List<FileResult> SelectedImageFiles { get; private set; } = new();

        [RelayCommand]
        public async Task AddImagesAsync()
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

                // image_paths most sima string -> pl. fájlnevek vesszővel elválasztva
                NewRecipe.image_paths = string.Join(",", files.Select(f => f.FileName));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hiba kép kiválasztásnál: {ex.Message}");
            }
        }

        [RelayCommand]
        async Task SaveRecipe()
        {
            string url = "https://bgs.jedlik.eu/hm/backend/public/api/recipes";
            using var client = new HttpClient();
            using var form = new MultipartFormDataContent();

            form.Add(new StringContent(NewRecipe.title), "title");
            form.Add(new StringContent(NewRecipe.description), "description");
            form.Add(new StringContent(NewRecipe.ingredients), "ingredients");
            form.Add(new StringContent(NewRecipe.avg_time), "avg_time");

            foreach (var file in SelectedImageFiles)
            {
                using var stream = await file.OpenReadAsync();
                using var ms = new MemoryStream();
                await stream.CopyToAsync(ms);
                var bytes = ms.ToArray();

                var fileContent = new ByteArrayContent(bytes);
                fileContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
                form.Add(fileContent, "image_paths[]", file.FileName);
            }

            var response = await client.PostAsync(url, form);

            if (response.IsSuccessStatusCode)
            {
                await Shell.Current.DisplayAlert("Siker", "Recept sikeresen mentve!", "ok");
                await Shell.Current.GoToAsync(nameof(RecipeListPage));
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Hiba: {response.StatusCode} - {errorContent}");
                await Shell.Current.DisplayAlert("Hiba", $"Sikertelen mentés: {response.StatusCode}", "Ok");
            }
        }
    }
}
