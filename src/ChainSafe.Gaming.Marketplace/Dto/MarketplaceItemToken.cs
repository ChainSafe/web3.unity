// <copyright file="MarketplaceItemToken.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ChainSafe.Gaming.Marketplace
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    /// <summary>
    /// Marketplace item token.
    /// </summary>
    public class MarketplaceItemToken
    {
        /// <summary>
        /// Gets or sets token Id.
        /// </summary>
        [JsonProperty(PropertyName = "token_id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets token type.
        /// </summary>
        [JsonProperty(PropertyName = "token_type")]
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets contract address.
        /// </summary>
        [JsonProperty(PropertyName = "contract_address")]
        public string ContractAddress { get; set; }

        /// <summary>
        /// Gets or sets uri.
        /// </summary>
        [JsonProperty(PropertyName = "uri")]
        public string Uri { get; set; }

        /// <summary>
        /// Gets or sets metadata.
        /// </summary>
        [JsonProperty(PropertyName = "metadata")]
        public Dictionary<string, object> Metadata { get; set; }
    }
}