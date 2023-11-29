using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;

using Newtonsoft.Json;

namespace MetaMask.Models
{
    public class MetaMaskTransaction : ISerializerCallback
    {

        [JsonProperty("to")]
        [JsonPropertyName("to")]
        public string To { get; set; }

        [JsonProperty("from")]
        [JsonPropertyName("from")]
        public string From { get; set; }

        [JsonProperty("value", DefaultValueHandling = DefaultValueHandling.Populate)]
        [JsonPropertyName("value")]
        public string Value { get; set; } = "0x0";
        
        [JsonProperty("data")]
        [JsonPropertyName("data")]
        public string Data { get; set; }
        
        [JsonProperty("chainId")]
        [JsonPropertyName("chainId")]
        public string ChainId { get; set; }

        public object OnSerialize()
        {
            if (Value.StartsWith("0x")) return this;
            
            BigInteger b = BigInteger.Parse(Value);
            Value = b.ToString("x");

            return this;
        }
    }
}
