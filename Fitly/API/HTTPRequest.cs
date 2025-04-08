using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace Fitly.API
{
    public class HTTPRequest<T> where T : class
    {
        private static readonly HttpClient client = new();

        private static string loginToken = SecureStorage.Default.GetAsync("LoginToken").Result;

        public static async Task<T?> Get(string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64)");
            // Authorization fejléc hozzáadása, ha van token
            if (loginToken != null)
            {
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", loginToken);
            }

            using var response = await client.SendAsync(request).ConfigureAwait(false);

           if (response.IsSuccessStatusCode)
            {
                string resultString = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<T>(resultString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }

            return null;
        }

        public static async Task<T> Post(string url, object data)
        {
            try
            {
                using var client = new HttpClient();
                var content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
                var request = new HttpRequestMessage(HttpMethod.Post, url) { Content = content };
                request.Headers.Add("Accept", "application/json");
                if (loginToken != null)
                {
                    request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", loginToken);
                }

                request.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64)");
                using var response = await client.SendAsync(request).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    string resultString = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<T>(resultString);
                }
            }
            catch (Exception ex)
            {
                await MainThread.InvokeOnMainThreadAsync(async () =>
                {
                    await Shell.Current.DisplayAlert("Hiba", "Hiba történt", "OK");
                });
            }
            return null;
        }

        public static async Task<T?> Put(string url, object data)
        {
            try
            {
                using var client = new HttpClient();
                var content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
                var request = new HttpRequestMessage(HttpMethod.Put, url) { Content = content };
                request.Headers.Add("Accept", "application/json");

                // Authorization fejléc hozzáadása, ha van token
                if (loginToken != null)
                {
                    request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", loginToken);
                }

                using var response = await client.SendAsync(request).ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    string resultString = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<T>(resultString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
            }
            catch (Exception ex)
            {
                await MainThread.InvokeOnMainThreadAsync(async () =>
                {
                    await Shell.Current.DisplayAlert("Hiba", "Hiba történt a PUT kérés során.", "OK");
                });
            }
            return null;
        }

        public static async Task<T?> SendPostRequest<T>(
            string url,
            object data,
            List<FileResult>? imageFiles = null)
        {
            try
            {
                using var client = new HttpClient();
                HttpContent content;

                if (imageFiles != null && imageFiles.Any())
                {
                    var form = new MultipartFormDataContent();

                    // Szöveges mezők: data objektum tulajdonságait bejárjuk
                    var props = data.GetType().GetProperties();
                    foreach (var prop in props)
                    {
                        var value = prop.GetValue(data)?.ToString() ?? string.Empty;
                        form.Add(new StringContent(value), prop.Name.ToLower());
                    }

                    // Képfájlok hozzáadása
                    foreach (var file in imageFiles)
                    {
                        using var stream = await file.OpenReadAsync();
                        using var ms = new MemoryStream();
                        await stream.CopyToAsync(ms);
                        var bytes = ms.ToArray();

                        var fileContent = new ByteArrayContent(bytes);
                        fileContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
                        form.Add(fileContent, "image_paths[]", file.FileName); // vagy a kulcs amit a backend vár
                    }

                    content = form;
                }
                else
                {
                    // JSON fallback
                    content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
                }

                var request = new HttpRequestMessage(HttpMethod.Post, url) { Content = content };
                request.Headers.Add("Accept", "application/json");
                request.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64)");

                if (!string.IsNullOrEmpty(loginToken))
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", loginToken);
                }

                using var response = await client.SendAsync(request).ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    string resultString = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<T>(resultString);
                }
            }
            catch (Exception ex)
            {
                await MainThread.InvokeOnMainThreadAsync(async () =>
                {
                    await Shell.Current.DisplayAlert("Hiba", "Hiba történt", "OK");
                });
            }

            return default;
        }

    }


}
