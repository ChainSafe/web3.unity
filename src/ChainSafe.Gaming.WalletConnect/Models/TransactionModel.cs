using Newtonsoft.Json;

namespace ChainSafe.Gaming.WalletConnect.Models
{
    public class TransactionModel
    {
        [JsonProperty("from")]
        public string From { get; set; }

        [JsonProperty("to")]
        public string To { get; set; }

        [JsonProperty("gas", NullValueHandling = NullValueHandling.Ignore)]
        public string Gas { get; set; }

        [JsonProperty("gasPrice", NullValueHandling = NullValueHandling.Ignore)]
        public string GasPrice { get; set; }

        [JsonProperty("value", NullValueHandling = NullValueHandling.Ignore)]
        public string Value { get; set; }

        [JsonProperty("data")]
        public string Data { get; set; }

        [JsonProperty("nonce", NullValueHandling = NullValueHandling.Ignore)]
        public string Nonce { get; set; }
    }
}