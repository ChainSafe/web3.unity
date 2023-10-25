using Newtonsoft.Json;

namespace ChainSafe.GamingSdk.Gelato.Types
{
    /// <summary>
    /// Class that represents a basic configuration for Gelato.
    /// </summary>
    public class GelatoConfig
    {
        /// <summary>
        ///    DATA - Relayer endpoint.
        /// </summary>
        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }

        /// <summary>
        ///     DATA - 1Balance api key.
        /// </summary>
        [JsonProperty(PropertyName = "sponsorApiKey")]
        public string SponsorApiKey { get; set; }

        /// <summary>
        ///     DATA - Address for Erc2771 relay contract.
        /// </summary>
        public string GelatoRelayErc2771Address { get; set; }

        /// <summary>
        ///     DATA - Address for Gelato 1Balance contract.
        /// </summary>
        public string GelatoRelay1BalanceErc2771Address { get; set; }

        /// <summary>
        ///     DATA - Address for Gelato ZKSync contract.
        /// </summary>
        public string GelatoRelayErc2771ZkSyncAddress { get; set; }

        /// <summary>
        ///     DATA - Address for Gelato 1Balance ZKSync contract.
        /// </summary>
        public string GelatoRelay1BalanceErc2771ZkSyncAddress { get; set; }
    }
}