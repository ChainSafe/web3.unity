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
        /// Gets & sets token Id.
        /// </summary>
        [JsonProperty(PropertyName = "token_id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets & sets token type.
        /// </summary>
        [JsonProperty(PropertyName = "token_type")]
        public string Type { get; set; }

        /// <summary>
        /// Gets & sets contract address.
        /// </summary>
        [JsonProperty(PropertyName = "contract_address")]
        public string ContractAddress { get; set; }

        /// <summary>
        /// Gets & sets uri.
        /// </summary>
        [JsonProperty(PropertyName = "uri")]
        public string Uri { get; set; }

        /// <summary>
        /// Gets & sets metadata.
        /// </summary>
        [JsonProperty(PropertyName = "metadata")]
        public Dictionary<string, string> Metadata { get; set; }
    }
}