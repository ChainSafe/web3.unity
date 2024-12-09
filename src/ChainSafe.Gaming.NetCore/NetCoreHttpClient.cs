using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ChainSafe.Gaming.Web3.Environment;
using ChainSafe.Gaming.Web3.Environment.Http;

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

        public async ValueTask<NetworkResponse<string>> GetRaw(string url, params HttpHeader[] headers)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            foreach (var header in headers)
            {
                request.Headers.Add(header.Name, header.Value);
            }

            var response = await originalClient.SendAsync(request);
            return await ResponseToNetworkResponse(response);
        }

        public async ValueTask<NetworkResponse<string>> PostRaw(
            string url,
            string data,
            string contentType,
            params HttpHeader[] headers)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            foreach (var header in headers)
            {
                request.Headers.Add(header.Name, header.Value);
            }

            request.Content = new StringContent(data, Encoding.UTF8, contentType);
            var response = await originalClient.SendAsync(request);
            return await ResponseToNetworkResponse(response);
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
    }
}