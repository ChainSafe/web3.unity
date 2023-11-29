using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace evm.net.Models
{
    public class EthCall
    {
        [JsonProperty("from")]
        [JsonPropertyName("from")]
        public string From { get; set; }

        [JsonProperty("to", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("to")]
        public string To { get; set; }

        [JsonProperty("gas", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("gas")]
        public string Gas { get; set; }

        [JsonProperty("gasPrice", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("gasPrice")]
        public string GasPrice { get; set; }

        [JsonProperty("value", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("value")]
        public string Value { get; set; }

        [JsonProperty("data")]
        [JsonPropertyName("data")]
        public string Data { get; set; }
    }
}