using Newtonsoft.Json;

namespace ChainSafe.GamingSdk.Gelato.Dto
{
    public class Erc2771UserParams
    {
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