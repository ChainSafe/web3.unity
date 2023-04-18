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
            using (var httpClient = new HttpClient())
            {
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(connection, content);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"HTTP Error {response.StatusCode}");
                }

                var jsonContent = await response.Content.ReadAsStringAsync();
                var serializer = JsonSerializer.Create();
                var message = serializer.Deserialize<T>(new JsonTextReader(new StringReader(jsonContent)));

                return message;
            }
        }

    }
}