using Newtonsoft.Json;

namespace ChainSafe.GamingSdk.Gelato.Relay
{
    public class OraclesResponse
    {
        /// <summary>
        ///     DATA, list of networks where the oracles are functional
        /// </summary>
        [JsonProperty(PropertyName = "oracles")]
        public string[] Oracles { get; set; }
    }
}