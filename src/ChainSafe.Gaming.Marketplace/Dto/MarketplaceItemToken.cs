// <copyright file="MarketplaceItemToken.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ChainSafe.Gaming.Marketplace
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class MarketplaceItemToken
    {
        /// <summary>
        /// Token Id
        /// </summary>
        [JsonProperty(PropertyName = "token_id")]
        public string Id { get; set; }

        /// <summary>
        /// Token type
        /// </summary>
        [JsonProperty(PropertyName = "token_type")]
        public string Type { get; set; }

        /// <summary>
        /// Contract address
        /// </summary>
        [JsonProperty(PropertyName = "contract_address")]
        public string ContractAddress { get; set; }

        /// <summary>
        /// Uri
        /// </summary>
        [JsonProperty(PropertyName = "uri")]
        public string Uri { get; set; }

        /// <summary>
        /// Metadata
        /// </summary>
        [JsonProperty(PropertyName = "metadata")]
        public Dictionary<string, string> Metadata { get; set; }
    }
}