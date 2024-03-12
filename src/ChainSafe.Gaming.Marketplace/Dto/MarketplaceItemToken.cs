// <copyright file="MarketplaceItemToken.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ChainSafe.Gaming.Marketplace
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class MarketplaceItemToken
    {
        [JsonProperty(PropertyName = "token_id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "token_type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "contract_address")]
        public string ContractAddress { get; set; }

        [JsonProperty(PropertyName = "uri")]
        public string Uri { get; set; }

        [JsonProperty(PropertyName = "metadata")]
        public Dictionary<string, string> Metadata { get; set; }
    }
}