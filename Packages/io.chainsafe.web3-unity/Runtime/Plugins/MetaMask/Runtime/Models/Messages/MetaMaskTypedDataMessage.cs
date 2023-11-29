using System.Text.Json.Serialization;
using evm.net.Models;
using Newtonsoft.Json;

namespace MetaMask.Models
{
    public class MetaMaskTypedDataMessage<T>
    {
        [JsonProperty("name")]
        [JsonPropertyName("name")]
        public string Name { get; set; }
        
        [JsonProperty("data")]
        [JsonPropertyName("data")]
        public T Data { get; set; }
    }
}