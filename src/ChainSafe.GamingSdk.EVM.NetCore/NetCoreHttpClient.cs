using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ChainSafe.GamingWeb3.Environment;
using Newtonsoft.Json;

namespace Web3Unity.Scripts.Library.Ethers.NetCore
{
    public class NetCoreHttpClient : IHttpClient
    {
        private readonly HttpClient originalClient;

        public NetCoreHttpClient()
        {
            originalClient = new HttpClient();
        }

        private static async ValueTask<NetworkResponse<string>> ResponseToNetworkResponse(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                return NetworkResponse<string>.Success(await response.Content.ReadAsStringAsync());
            }
            else
            {
                return NetworkResponse<string>.Failure($"{response.StatusCode}: {await response.Content.ReadAsStringAsync()}");
            }
        }

        public async ValueTask<NetworkResponse<string>> GetRaw(string url)
        {
            var response = await originalClient.GetAsync(url);
            return await ResponseToNetworkResponse(response);
        }

        public async ValueTask<NetworkResponse<string>> PostRaw(string url, string data, string contentType)
        {
            var requestContent = new StringContent(data, Encoding.UTF8, contentType);
            var response = await originalClient.PostAsync(url, requestContent);
            return await ResponseToNetworkResponse(response);
        }

        public async ValueTask<NetworkResponse<TResponse>> Get<TResponse>(string url)
        {
            var response = await GetRaw(url);
            return response.Map(x => JsonConvert.DeserializeObject<TResponse>(x));
        }

        public async ValueTask<NetworkResponse<TResponse>> Post<TRequest, TResponse>(string url, TRequest data)
        {
            var requestJson = JsonConvert.SerializeObject(data);
            var response = await PostRaw(url, requestJson, "application/json");
            return response.Map(x => JsonConvert.DeserializeObject<TResponse>(x));
        }
    }
}