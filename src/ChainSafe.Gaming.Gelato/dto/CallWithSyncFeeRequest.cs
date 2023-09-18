using ChainSafe.GamingSdk.Gelato.Types;
using Newtonsoft.Json;

namespace ChainSafe.GamingSdk.Gelato.Dto
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class CallWithSyncFeeRequest : RelayRequestOptions
    {
        /// <summary>
        ///    QUANTITY - The transaction chain id.
        /// </summary>
        [JsonProperty(PropertyName = "chainId")]
        public int ChainId { get; set; }

        /// <summary>
        ///     DATA, 20 Bytes - The address the transaction is being sent to.
        /// </summary>
        [JsonProperty(PropertyName = "target")]
        public string Target { get; set; }

        /// <summary>
        ///     DATA - the data send along with the transaction.
        /// </summary>
        [JsonProperty(PropertyName = "data")]
        public string Data { get; set; }

        /// <summary>
        ///     DATA, 20 Bytes - the address of the token that is to be used for payment.
        /// </summary>
        [JsonProperty(PropertyName = "feeToken")]
        public string FeeToken { get; set; }

        /// <summary>
        ///     DATA - an optional boolean (default: true ) denoting what data you would prefer appended to the end of the calldata.
        /// </summary>
        [JsonProperty(PropertyName = "isRelayContext")]
        public bool IsRelayContext { get; set; }
    }
}