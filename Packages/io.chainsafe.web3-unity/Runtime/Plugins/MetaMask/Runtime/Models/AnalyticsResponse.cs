using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace MetaMask.Models
{
    public class AnalyticsResponse
    {
        [JsonProperty("success"), JsonPropertyName("success")]
        public bool Success { get; set; }
    }
}