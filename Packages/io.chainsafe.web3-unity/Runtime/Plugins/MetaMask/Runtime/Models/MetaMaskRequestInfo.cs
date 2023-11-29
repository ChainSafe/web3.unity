using System.Text.Json.Serialization;

using Newtonsoft.Json;

namespace MetaMask.Models
{
    public class MetaMaskRequestInfo
    {

        [JsonProperty("type")]
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonProperty("originatorInfo")]
        [JsonPropertyName("originatorInfo")]
        public MetaMaskOriginatorInfo OriginatorInfo { get; set; }

    }
}
