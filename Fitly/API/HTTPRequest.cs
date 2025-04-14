using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace Fitly.API
{
    public static class HTTPRequest<T> where T : class
    {
        private static readonly HttpClient client = new();


        public static async Task<T?> Get(string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64)");
            var loginToken = await SecureStorage.Default.GetAsync("LoginToken");
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
                var loginToken = await SecureStorage.Default.GetAsync("LoginToken");
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
                var loginToken = await SecureStorage.Default.GetAsync("LoginToken");

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

        public static async Task<T?> Delete(string url)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Delete, url);
                request.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64)");
                var loginToken = await SecureStorage.Default.GetAsync("LoginToken");

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
                else
                {
                    Console.WriteLine($"Hiba a DELETE kérés során: {response.StatusCode}");
                    await MainThread.InvokeOnMainThreadAsync(async () =>
                    {
                        await Shell.Current.DisplayAlert("Hiba", $"Sikertelen törlés!", "OK");
                    });
                }

            }
            catch (Exception)
            {
                await MainThread.InvokeOnMainThreadAsync(async () =>
                {
                    await Shell.Current.DisplayAlert("Hiba", "Váratlan hiba történt a törlés során.", "OK");
                });
            }
            return null;
        }
    }
}
