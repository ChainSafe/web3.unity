using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using System.Web;
using ChainSafe.Gaming.Evm.Contracts;
using ChainSafe.Gaming.Evm.Transactions;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Environment;
using Nethereum.Hex.HexTypes;

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
        private IContractBuilder contractBuilder;

        public MarketplaceClient(IHttpClient httpClient, IProjectConfig projectConfig, IChainConfig chainConfig, IContractBuilder contractBuilder)
            : this(httpClient, projectConfig, chainConfig, contractBuilder, MarketplaceConfig.Default)
        {
        }

        public MarketplaceClient(IHttpClient httpClient, IProjectConfig projectConfig, IChainConfig chainConfig,
            IContractBuilder contractBuilder, IMarketplaceConfig config)
        {
            this.contractBuilder = contractBuilder;
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

        public Task Purchase(string marketplaceContract, string itemId, string itemPrice)
        {
            var itemIdInt = BigInteger.Parse(itemId);
            var priceInt = BigInteger.Parse(itemPrice);
            return Purchase(marketplaceContract, itemIdInt, priceInt);
        }

        public async Task Purchase(string marketplaceContract, BigInteger itemId, BigInteger itemPrice)
        {
            var contract = contractBuilder.Build(config.MarketplaceContractAbi, marketplaceContract);
            var transactionPrototype = new TransactionRequest { Value = new HexBigInteger(itemPrice) };
            await contract.Send("purchaseItem", new object[] { itemId }, transactionPrototype);
        }
    }
}