using Newtonsoft.Json;

namespace ChainSafe.GamingSdk.Gelato.Dto
{
    public class SupportedNetworksResponse
    {
        /// <summary>
        ///     DATA, list of chain Ids the relayers are running on.
        /// </summary>
        [JsonProperty(PropertyName = "relays")]
        public string[] Relays { get; set; }
    }
}