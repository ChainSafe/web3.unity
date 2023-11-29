using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace MetaMask.Models
{
    public class ChainData
    {
        [JsonProperty("networkVersion")]
        [JsonPropertyName("networkVersion")]
        public string NetworkVersion { get; set; }
        
        [JsonProperty("chainId")]
        [JsonPropertyName("chainId")]
        public string ChainId { get; set; }
    }
}