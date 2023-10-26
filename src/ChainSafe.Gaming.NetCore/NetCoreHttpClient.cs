using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ChainSafe.Gaming.Web3.Environment;
using Newtonsoft.Json;

namespace ChainSafe.Gaming.NetCore
{
    /// <summary>
    /// Implementation of <see cref="IHttpClient"/> for NetCore environment.
    /// </summary>
    public class NetCoreHttpClient : IHttpClient
    {
        private readonly HttpClient originalClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="NetCoreHttpClient"/> class.
        /// </summary>
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

        /// <summary>
        /// Get Raw response from http request with a GET method.
        /// </summary>
        /// <param name="url">Url of request to be made.</param>
        /// <returns>Raw response to the request.</returns>
        public async ValueTask<NetworkResponse<string>> GetRaw(string url)
        {
            var response = await originalClient.GetAsync(url);
            return await ResponseToNetworkResponse(response);
        }

        /// <summary>
        /// Get Raw response from http request with a POST method.
        /// </summary>
        /// <param name="url">Url of request to be made.</param>
        /// <param name="data">Request data/body.</param>
        /// <param name="contentType">Content type request header.</param>
        /// <returns>Raw response to the request.</returns>
        public async ValueTask<NetworkResponse<string>> PostRaw(string url, string data, string contentType)
        {
            var requestContent = new StringContent(data, Encoding.UTF8, contentType);
            var response = await originalClient.PostAsync(url, requestContent);
            return await ResponseToNetworkResponse(response);
        }

        /// <summary>
        /// Get response as <see cref="TResponse"/> from http request with a GET method.
        /// </summary>
        /// <param name="url">Url of request to be made.</param>
        /// <typeparam name="TResponse">Response body model type.</typeparam>
        /// <returns>Response as a <see cref="TResponse"/>.</returns>
        public async ValueTask<NetworkResponse<TResponse>> Get<TResponse>(string url)
        {
            var response = await GetRaw(url);
            return response.Map(x => JsonConvert.DeserializeObject<TResponse>(x));
        }

        /// <summary>
        /// Get response as <see cref="TResponse"/> from http request with a GET method.
        /// </summary>
        /// <param name="url">Url of request to be made.</param>
        /// <param name="data">Request data/body.</param>
        /// <typeparam name="TRequest">Request data/body model type.</typeparam>
        /// <typeparam name="TResponse">Response data/body type.</typeparam>
        /// <returns>Response as a <see cref="TResponse"/>.</returns>
        public async ValueTask<NetworkResponse<TResponse>> Post<TRequest, TResponse>(string url, TRequest data)
        {
            var requestJson = JsonConvert.SerializeObject(data);
            var response = await PostRaw(url, requestJson, "application/json");
            return response.Map(x => JsonConvert.DeserializeObject<TResponse>(x));
        }
    }
}