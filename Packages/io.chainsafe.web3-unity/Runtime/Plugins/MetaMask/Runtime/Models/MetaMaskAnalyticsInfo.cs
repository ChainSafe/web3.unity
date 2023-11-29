using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace MetaMask.Models
{
    public class MetaMaskAnalyticsInfo
    {

        [JsonProperty("id")]
        [JsonPropertyName("id")]
        public string Id = "sdk";

        [JsonProperty("event")] [JsonPropertyName("event")]
        public string Event;

        [JsonProperty("commLayer")]
        [JsonPropertyName("commLayer")]
        public string CommunicationLayerPreference = "socket";

        [JsonProperty("sdkVersion")]
        [JsonPropertyName("sdkVersion")]
        public string SdkVersion = MetaMaskWallet.Version;

        [JsonProperty("originatorInfo")] [JsonPropertyName("originatorInfo")]
        public MetaMaskOriginatorInfo OriginatorInfo;
        
        [JsonProperty("platform")] [JsonPropertyName("platform")]
        public string Platform;
    }
}
