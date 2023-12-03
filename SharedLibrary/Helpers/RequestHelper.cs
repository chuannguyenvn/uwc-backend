using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Commons.Helpers
{
    public static class RequestHelper
    {
        public static async Task Get(string url)
        {
            url = "https://" + url;
            var client = new HttpClient();
            await client.GetAsync(url);
        }

        public static async Task<T> Get<T>(string url)
        {
            url = "https://" + url;
            var client = new HttpClient();
            var response = await client.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(content);
        }

        public static async Task Post(string url, object body)
        {
            url = "https://" + url;
            var client = new HttpClient();
            await client.PostAsync(url, new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json"));
        }

        public static async Task<T> Post<T>(string url, object body)
        {
            url = "https://" + url;
            var client = new HttpClient();
            var response = await client.PostAsync(url, new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json"));
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(content);
        }
    }
}