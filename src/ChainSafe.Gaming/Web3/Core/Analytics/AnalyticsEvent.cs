using Newtonsoft.Json;

namespace ChainSafe.Gaming.Web3.Analytics
{
    public class AnalyticsEvent
    {
        [JsonProperty(PropertyName = "projectId", NullValueHandling = NullValueHandling.Ignore)]
        public string ProjectId { get; set; }

        [JsonProperty(PropertyName = "chain", NullValueHandling = NullValueHandling.Ignore)]
        public string ChainId { get; set; }

        [JsonProperty(PropertyName = "rpc", NullValueHandling = NullValueHandling.Ignore)]
        public string Rpc { get; set; }

        [JsonProperty(PropertyName = "network", NullValueHandling = NullValueHandling.Ignore)]
        public string Network { get; set; }

        [JsonProperty(PropertyName = "version", NullValueHandling = NullValueHandling.Ignore)]
        public string Version { get; set; }

        [JsonProperty(PropertyName = "gameData", NullValueHandling = NullValueHandling.Ignore)]
        public AnalyticsGameData GameData { get; set; }
    }
}