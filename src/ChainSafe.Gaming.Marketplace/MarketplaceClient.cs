using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Environment;

namespace ChainSafe.Gaming.Marketplace
{
    /*
     * todo add method wrappers for "Get Marketplace Items" and "Get Item" also
     * https://docs.gaming.chainsafe.io/marketplace-api/docs/marketplaceapi/#tag/Items/operation/getItem
     */

    public class MarketplaceClient
    {
        private readonly IMarketplaceConfig config;
        private readonly IHttpClient httpClient;
        private IProjectConfig projectConfig;
        private IChainConfig chainConfig;

        public MarketplaceClient(IHttpClient httpClient, IProjectConfig projectConfig, IChainConfig chainConfig)
            : this(httpClient, projectConfig, chainConfig, MarketplaceConfig.Default)
        {
        }

        public MarketplaceClient(IHttpClient httpClient, IProjectConfig projectConfig, IChainConfig chainConfig, IMarketplaceConfig config)
        {
            this.chainConfig = chainConfig;
            this.httpClient = httpClient;
            this.config = config;
            this.projectConfig = projectConfig;
        }

        public Task<MarketplacePage> LoadPage(MarketplacePage? prevPage = null)
        {
            return LoadPage(prevPage?.Cursor ?? null);
        }

        public async Task<MarketplacePage> LoadPage(string? cursor)
        {
            var endpoint = config.EndpointOverride;
            if (endpoint.EndsWith('/'))
            {
                endpoint = endpoint.Substring(0, endpoint.LastIndexOf('/'));
            }

            var projectId = projectConfig.ProjectId;
            var baseUri = $"{endpoint}/v1/projects/{projectId}/items";
            var queryParameters = new Dictionary<string, string>
            {
                ["chainId"] = chainConfig.ChainId,
            };

            if (!string.IsNullOrEmpty(cursor))
            {
                queryParameters["cursor"] = cursor;
            }

            var queryParametersString = queryParameters
                .Select(p => $"{p.Key}={HttpUtility.UrlEncode(p.Value)}");
            var query = string.Join('&', queryParametersString);
            var uri = $"{baseUri}?{query}";

            var response = await httpClient.Get<MarketplacePage>(uri);
            return response.AssertSuccess();
        }
    }
}