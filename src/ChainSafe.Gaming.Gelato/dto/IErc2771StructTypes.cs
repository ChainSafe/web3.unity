using System.Numerics;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Newtonsoft.Json;

namespace ChainSafe.GamingSdk.Gelato.Dto
{
    public interface IErc2771StructTypes
    {
        /// <summary>
        ///    QUANTITY - The transaction chain id.
        /// </summary>
        [Parameter("uint256", "chainId", 1)]
        [JsonProperty(PropertyName = "chainId")]
        public BigInteger? ChainId { get; set; }

        /// <summary>
        ///     DATA, 20 Bytes - The address the transaction is being sent to.
        /// </summary>
        [Parameter("address", "target", 2)]
        [JsonProperty(PropertyName = "target")]
        public string Target { get; set; }

        /// <summary>
        ///     DATA - the data send along with the transaction.
        /// </summary>
        [Parameter("bytes", "data", 3)]
        [JsonProperty(PropertyName = "data")]
        public string Data { get; set; }

        /// <summary>
        ///     DATA, 20 Bytes - the address of the user's EOA.
        /// </summary>
        [Parameter("address", "user", 4)]
        [JsonProperty(PropertyName = "user")]
        public string User { get; set; }

        /// <summary>
        ///    QUANTITY - optional, this is a nonce similar to Ethereum nonces, stored in a local mapping on the relay contracts. It is used to enforce nonce ordering of relay calls, if the user requires it. Otherwise, this is an optional parameter and if not passed, the relay-SDK will automatically query on-chain for the current value.
        /// </summary>
        [Parameter("uint256", "userNonce", 5)]
        [JsonProperty(PropertyName = "userNonce")]
        public BigInteger? UserNonce { get; set; }

        /// <summary>
        ///    QUANTITY - optional, the amount of time in seconds that a user is willing for the relay call to be active in the relay backend before it is dismissed.
        /// </summary>
        [Parameter("uint256", "userDeadline", 6)]
        [JsonProperty(PropertyName = "userDeadline")]
        public BigInteger? UserDeadline { get; set; }
    }
}