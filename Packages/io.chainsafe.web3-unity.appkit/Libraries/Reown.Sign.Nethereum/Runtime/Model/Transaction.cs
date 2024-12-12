using Newtonsoft.Json;
using Reown.Core.Common.Utils;

namespace Reown.Sign.Nethereum.Model
{
    public class Transaction
    {
        [JsonProperty("from")]
        public string from;

        [JsonProperty("to")]
        public string to;

        [JsonProperty("gas", NullValueHandling = NullValueHandling.Ignore)]
        public string gas;

        [JsonProperty("gasPrice", NullValueHandling = NullValueHandling.Ignore)]
        public string gasPrice;

        [JsonProperty("value")]
        public string value;

        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
        public string data = "0x";
        
        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string type;

        [Preserve]
        public Transaction()
        {
        }
    }
}