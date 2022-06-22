using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Web3Unity.Scripts.Library.Ethers.Web
{
    public class Web
    {
        public static async Task<T> FetchJson<T>(string connection, string json)
        {
            var httpClient = new HttpClient();
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var httpResponseMessage = await httpClient.PostAsync(connection, httpContent).ConfigureAwait(false);
            httpResponseMessage.EnsureSuccessStatusCode();

            var stream = await httpResponseMessage.Content.ReadAsStreamAsync();
            var streamReader = new StreamReader(stream);
            var reader = new JsonTextReader(streamReader);
            var serializer = JsonSerializer.Create();
            var message = serializer.Deserialize<T>(reader);

            return message;
        }
    }
}