using System.Collections.Generic;
using Newtonsoft.Json;

namespace ChainSafe.Gaming.Marketplace
{
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