// <copyright file="MarketplaceClient.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ChainSafe.Gaming.Marketplace
{
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

    /*
     * todo add method wrappers for "Get Marketplace Items" and "Get Item" also
     * https://docs.gaming.chainsafe.io/marketplace-api/docs/marketplaceapi/#tag/Items/operation/getItem
     */

    /// <summary>
    /// Marketplace client.
    /// </summary>
    public class MarketplaceClient
    {
        private readonly IMarketplaceConfig config;
        private readonly IHttpClient httpClient;
        private IProjectConfig projectConfig;
        private IChainConfig chainConfig;
        private IContractBuilder contractBuilder;

        /// <summary>
        /// Initializes a new instance of the <see cref="MarketplaceClient"/> class.
        /// Constructor creates an instance of the marketplace client class.
        /// </summary>
        /// <param name="httpClient">Http client.</param>
        /// <param name="projectConfig">Project config.</param>
        /// <param name="chainConfig">Chain config.</param>
        /// <param name="contractBuilder">Contract builder.</param>
        public MarketplaceClient(IHttpClient httpClient, IProjectConfig projectConfig, IChainConfig chainConfig, IContractBuilder contractBuilder)
            : this(httpClient, projectConfig, chainConfig, contractBuilder, MarketplaceConfig.Default)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MarketplaceClient"/> class.
        /// Constructor creates an instance of the marketplace client class with config overload.
        /// </summary>
        /// <param name="httpClient">Http client.</param>
        /// <param name="projectConfig">Project config.</param>
        /// <param name="chainConfig">Chain config.</param>
        /// <param name="contractBuilder">Contract builder.</param>
        /// <param name="config">Overload config.</param>
        public MarketplaceClient(IHttpClient httpClient, IProjectConfig projectConfig, IChainConfig chainConfig, IContractBuilder contractBuilder, IMarketplaceConfig config)
        {
            this.contractBuilder = contractBuilder;
            this.chainConfig = chainConfig;
            this.httpClient = httpClient;
            this.config = config;
            this.projectConfig = projectConfig;
        }

        /// <summary>
        /// Loads marketplace page via previous page.
        /// </summary>
        /// <param name="prevPage">Previous page.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public Task<MarketplacePage> LoadPage(MarketplacePage? prevPage = null)
        {
            return this.LoadPage(prevPage?.Cursor ?? null);
        }

        /// <summary>
        /// Loads marketplace page via cursor.
        /// </summary>
        /// <param name="cursor">Cursor.</param>
        /// <returns>Loaded page.</returns>
        public async Task<MarketplacePage> LoadPage(string? cursor)
        {
            var endpoint = this.config.EndpointOverride;
            if (endpoint.EndsWith('/'))
            {
                endpoint = endpoint.Substring(0, endpoint.LastIndexOf('/'));
            }

            var projectId = this.projectConfig.ProjectId;
            var baseUri = $"{endpoint}/v1/projects/{projectId}/items";
            var queryParameters = new Dictionary<string, string>
            {
                ["chainId"] = this.chainConfig.ChainId,
            };

            if (!string.IsNullOrEmpty(cursor))
            {
                queryParameters["cursor"] = cursor;
            }

            var queryParametersString = queryParameters
                .Select(p => $"{p.Key}={HttpUtility.UrlEncode(p.Value)}");
            var query = string.Join('&', queryParametersString);
            var uri = $"{baseUri}?{query}";

            var response = await this.httpClient.Get<MarketplacePage>(uri);
            return response.AssertSuccess();
        }

        /// <summary>
        /// Purchase a marketplace item with string datatypes, falls back into the overloaded call with big int data types.
        /// </summary>
        /// <param name="marketplaceContract">The marketplace contract.</param>
        /// <param name="itemId">Item id inteifier.</param>
        /// <param name="itemPrice">Price of the item.</param>
        /// <returns>Result of the purchase funtion.</returns>
        public Task Purchase(string marketplaceContract, string itemId, string itemPrice)
        {
            var itemIdInt = BigInteger.Parse(itemId);
            var priceInt = BigInteger.Parse(itemPrice);
            return this.Purchase(marketplaceContract, itemIdInt, priceInt);
        }

        /// <summary>
        /// Purchase a marketplace item with big integer datatypes.
        /// </summary>
        /// <param name="marketplaceContract">The marketplace contract.</param>
        /// <param name="itemId">Item id inteifier.</param>
        /// <param name="itemPrice">Price of the item.</param>
        /// <returns>A contract purchase.</returns>
        public async Task Purchase(string marketplaceContract, BigInteger itemId, BigInteger itemPrice)
        {
            var contract = this.contractBuilder.Build(this.config.MarketplaceContractAbi, marketplaceContract);
            var transactionPrototype = new TransactionRequest { Value = new HexBigInteger(itemPrice) };
            await contract.Send("purchaseItem", new object[] { itemId }, transactionPrototype);
        }
    }
}