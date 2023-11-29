using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace evm.net.Models
{
    public class EthSendTransaction : EthCall
    {
        [JsonProperty("nonce", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("nonce")]
        public string Nonce { get; set; }
    }
}