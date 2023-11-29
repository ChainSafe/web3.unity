using System.Text.Json.Serialization;
using evm.net.Models;
using Newtonsoft.Json;

namespace MetaMask.Models
{
    public class MetaMaskDataMessage
    {
        [JsonProperty("name")]
        [JsonPropertyName("name")]
        public string Name { get; set; }
        
        [JsonProperty("data")]
        [JsonPropertyName("data")]
        public JsonRpcPayload Data { get; set; }
    }
}