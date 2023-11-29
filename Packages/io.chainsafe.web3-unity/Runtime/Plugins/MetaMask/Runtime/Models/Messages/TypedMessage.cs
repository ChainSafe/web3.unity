using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace MetaMask.Models
{
    public class TypedMessage
    {
        [JsonProperty("type")]
        [JsonPropertyName("type")]
        public string Type { get; set; }
    }
}