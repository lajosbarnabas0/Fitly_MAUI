using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Fitly.API
{
    public class HTTPRequest<T> where T : class
    {
        private static readonly HttpClient client = new();

        public static async Task<T?> Get(string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            using var response = await client.SendAsync(request).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                string resultString = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<T>(resultString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }

            return null;
        }

        public static async Task<T?> Post(string url, object data)
        {
            var content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Post, url) { Content = content };
            using var response = await client.SendAsync(request).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                string resultString = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<T>(resultString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }

            return null;
        }
    }
}
