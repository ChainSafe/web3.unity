using Newtonsoft.Json;

namespace ChainSafe.GamingSdk.Gelato.Types
{
    public class ApiKey
    {
        /// <summary>
        ///     DATA - api key of the 1Balance account that is sponsoring the transaction.
        /// </summary>
        [JsonProperty(PropertyName = "sponsorApiKey")]
        public string SponsorApiKey { get; set; }
    }
}
