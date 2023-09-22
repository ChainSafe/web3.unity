using System.Collections.Generic;
using Newtonsoft.Json;

namespace ChainSafe.Gaming.Web3.Analytics
{
    public class AnalyticsGameData
    {
        [JsonProperty(PropertyName = "client", NullValueHandling = NullValueHandling.Ignore)]
        public string Client { get; set; }

        [JsonProperty(PropertyName = "player", NullValueHandling = NullValueHandling.Ignore)]
        public string Player { get; set; }

        [JsonProperty(PropertyName = "to", NullValueHandling = NullValueHandling.Ignore)]
        public string To { get; set; }

        [JsonProperty(PropertyName = "value", NullValueHandling = NullValueHandling.Ignore)]
        public string Value { get; set; }

        [JsonProperty(PropertyName = "gas_limit", NullValueHandling = NullValueHandling.Ignore)]
        public string GasLimit { get; set; }

        [JsonProperty(PropertyName = "gas_price", NullValueHandling = NullValueHandling.Ignore)]
        public string GasPrice { get; set; }

        [JsonProperty(PropertyName = "custom", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, object> CustomProperties { get; set; }

        [JsonProperty(PropertyName = "params", NullValueHandling = NullValueHandling.Ignore)]
        public object[] Params { get; set; }
    }
}