using Newtonsoft.Json;

namespace ChainSafe.Gaming.Web3.Analytics
{
    public class AnalyticsEvent
    {
        [JsonProperty(PropertyName = "gameData", NullValueHandling = NullValueHandling.Ignore)]
        public AnalyticsGameData GameData { get; set; }

        [JsonProperty(PropertyName = "eventName", NullValueHandling = NullValueHandling.Ignore)]
        public string EventName { get; set; }

        [JsonProperty(PropertyName = "packageName", NullValueHandling = NullValueHandling.Ignore)]
        public string PackageName { get; set; }
    }
}