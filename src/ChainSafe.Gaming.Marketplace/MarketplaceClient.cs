using System.Threading.Tasks;
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

        public MarketplaceClient(IHttpClient httpClient, IProjectConfig projectConfig)
            : this(httpClient, projectConfig, MarketplaceConfig.Default)
        {
        }

        public MarketplaceClient(IHttpClient httpClient, IProjectConfig projectConfig, IMarketplaceConfig config)
        {
            this.httpClient = httpClient;
            this.config = config;
            this.projectConfig = projectConfig;
        }

        public Task<MarketplacePage> LoadPage(MarketplacePage? prevPage = null)
        {
            return LoadPage(prevPage?.Cursor ?? null);
        }

        public async Task<MarketplacePage> LoadPage(string? cursor = null)
        {
            var endpoint = config.EndpointOverride;
            if (endpoint.EndsWith('/'))
            {
                endpoint = endpoint.Substring(0, endpoint.LastIndexOf('/'));
            }

            var projectId = projectConfig.ProjectId;
            var uri = $"{endpoint}/v1/projects/{projectId}/items";
            var response = await httpClient.Get<MarketplacePage>(uri);
            return response.AssertSuccess();
        }
    }
}