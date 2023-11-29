using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace MetaMask.Models
{
    public class ProviderState
    {
        [JsonProperty("accounts")]
        [JsonPropertyName("accounts")]
        public string[] Accounts { get; set; }
        
        [JsonProperty("chainId")]
        [JsonPropertyName("chainId")]
        public string ChainId { get; set; }
    }
}