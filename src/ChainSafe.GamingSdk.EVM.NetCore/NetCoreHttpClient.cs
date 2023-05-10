using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ChainSafe.GamingWeb3.Environment;
using Newtonsoft.Json;

namespace Web3Unity.Scripts.Library.Ethers.NetCore
{
    public class NetCoreHttpClient : IHttpClient
    {
        private readonly HttpClient _originalClient;

        public NetCoreHttpClient()
        {
            _originalClient = new HttpClient();
        }

        public async ValueTask<string> GetRaw(string url)
        {
            var response = await _originalClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var responseText = await response.Content.ReadAsStringAsync();
            return responseText;
        }

        public async ValueTask<string> PostRaw(string url, string data, string contentType)
        {
            var requestContent = new StringContent(data, Encoding.UTF8, contentType);
            var responseMessage = await _originalClient.PostAsync(url, requestContent);
            responseMessage.EnsureSuccessStatusCode();
            var responseText = await responseMessage.Content.ReadAsStringAsync();
            return responseText;
        }

        public async ValueTask<TResponse> Get<TResponse>(string url)
        {
            var responseJson = await GetRaw(url);
            var responseData = JsonConvert.DeserializeObject<TResponse>(responseJson);
            return responseData;
        }

        public async ValueTask<TResponse> Post<TRequest, TResponse>(string url, TRequest data)
        {
            var requestJson = JsonConvert.SerializeObject(data);
            var responseJson = await PostRaw(url, requestJson, "application/json");
            var responseData = JsonConvert.DeserializeObject<TResponse>(responseJson);
            return responseData;
        }
    }
}