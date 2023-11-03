using Newtonsoft.Json;

namespace ChainSafe.Gaming.WalletConnect.Models
{
    /// <summary>
    /// Transaction Model used for Wallet Connect Json RPC params, see https://docs.walletconnect.com/advanced/rpc-reference/ethereum-rpc#eth_sendtransaction.
    /// </summary>
    public class TransactionModel
    {
        /// <summary>
        /// Transaction Sender's public address.
        /// </summary>
        [JsonProperty("from")]
        public string From { get; set; }

        /// <summary>
        /// Transaction Receiver's public address.
        /// </summary>
        [JsonProperty("to")]
        public string To { get; set; }

        /// <summary>
        /// Gas fee for transaction in Hex Value.
        /// </summary>
        [JsonProperty("gas", NullValueHandling = NullValueHandling.Ignore)]
        public string Gas { get; set; }

        /// <summary>
        /// Price of <see cref="Gas"/> in Hex Value.
        /// </summary>
        [JsonProperty("gasPrice", NullValueHandling = NullValueHandling.Ignore)]
        public string GasPrice { get; set; }

        /// <summary>
        /// Amount to be sent in Hex Value.
        /// </summary>
        [JsonProperty("value", NullValueHandling = NullValueHandling.Ignore)]
        public string Value { get; set; }

        /// <summary>
        /// Transaction Data if any.
        /// </summary>
        [JsonProperty("data")]
        public string Data { get; set; }

        /// <summary>
        /// Nonce in Hex Value.
        /// </summary>
        [JsonProperty("nonce", NullValueHandling = NullValueHandling.Ignore)]
        public string Nonce { get; set; }
    }
}