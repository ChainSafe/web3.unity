using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace MetaMask.Models
{
    public class MetaMaskPing
    {
        [JsonProperty("type")]
        [JsonPropertyName("type")]
        public string Type { get; }

        public MetaMaskPing()
        {
            this.Type = "ping";
        }
    }
}