using ChainSafe.GamingSdk.Gelato.Types;
using Newtonsoft.Json;

namespace ChainSafe.GamingSdk.Gelato.Dto
{
    // CallWithERC2771Request & BaseCallWithSyncFeeParams;
    public class CallWithSyncFeeErc2771Request : RelayRequestOptions
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

        /// <summary>
        ///     DATA, 20 Bytes - the address of the user's EOA.
        /// </summary>
        [JsonProperty(PropertyName = "user")]
        public string User { get; set; }

        /// <summary>
        ///    QUANTITY - optional, this is a nonce similar to Ethereum nonces, stored in a local mapping on the relay contracts. It is used to enforce nonce ordering of relay calls, if the user requires it. Otherwise, this is an optional parameter and if not passed, the relay-SDK will automatically query on-chain for the current value.
        /// </summary>
        [JsonProperty(PropertyName = "userNonce")]
        public int? UserNonce { get; set; }

        /// <summary>
        ///    QUANTITY - optional, the amount of time in seconds that a user is willing for the relay call to be active in the relay backend before it is dismissed.
        /// </summary>
        [JsonProperty(PropertyName = "userDeadline")]
        public int? UserDeadline { get; set; }
    }
}