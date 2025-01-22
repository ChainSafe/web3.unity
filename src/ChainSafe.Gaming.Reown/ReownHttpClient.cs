using System.Linq;
using System.Threading.Tasks;
using ChainSafe.Gaming.Web3.Environment;
using ChainSafe.Gaming.Web3.Environment.Http;

namespace ChainSafe.Gaming.Reown
{
    /// <summary>
    /// An HTTP client wrapper that adds the HTTP headers required by the Reown API to each request.
    /// </summary>
    public class ReownHttpClient : IHttpClient
    {
        public const string Host = "https://api.web3modal.com";

        private readonly IHttpClient originalClient;
        private readonly HttpHeader[] reownHeaders;
        private readonly IReownConfig reownConfig;

        public ReownHttpClient(IHttpClient originalClient, IReownConfig reownConfig)
        {
            this.reownConfig = reownConfig;
            this.originalClient = originalClient;

            reownConfig.Validate();

            reownHeaders = BuildHeaders();
        }

        public HttpHeader[] BuildHeaders()
        {
            return new[]
            {
                new HttpHeader { Name = "x-project-id", Value = reownConfig.ProjectId },
                new HttpHeader { Name = "x-sdk-type", Value = "appkit" },
                new HttpHeader { Name = "x-sdk-version", Value = "unity-appkit-v1.0.0" },
            };
        }

        public ValueTask<NetworkResponse<string>> GetRaw(string url, params HttpHeader[] headers)
        {
            return originalClient.GetRaw(url, AppendReownHeaders(headers));
        }

        public ValueTask<NetworkResponse<string>> PostRaw(string url, string data, string contentType, params HttpHeader[] headers)
        {
            return originalClient.PostRaw(url, data, contentType, AppendReownHeaders(headers));
        }

        private HttpHeader[] AppendReownHeaders(HttpHeader[] originalHeaders)
        {
            return reownHeaders.Concat(originalHeaders).ToArray();
        }
    }
}