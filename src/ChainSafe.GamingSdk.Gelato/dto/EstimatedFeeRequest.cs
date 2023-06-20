using Nethereum.Hex.HexTypes;
using Newtonsoft.Json;

namespace ChainSafe.GamingSdk.Gelato.Dto
{
    public class EstimatedFeeRequest
    {
        /// <summary>
        ///     DATA,  Id of the chain the transaction is being relayed to.
        /// </summary>
        [JsonProperty(PropertyName = "chainId")]
        public string ChainId { get; set; }

        /// <summary>
        ///     DATA,  Address of the ERC20 token being used to fund the transaction.
        /// </summary>
        [JsonProperty(PropertyName = "paymentToken")]
        public string PaymentToken { get; set; }

        /// <summary>
        ///     DATA,  Gas Limit for the transaction being relayed.
        /// </summary>
        [JsonProperty(PropertyName = "gasLimit")]
        public HexBigInteger GasLimit { get; set; }

        /// <summary>
        ///     DATA,  Indicates priority level of the transaction.
        /// </summary>
        [JsonProperty(PropertyName = "isHighPriority")]
        public bool IsHighPriority { get; set; }

        /// <summary>
        ///     DATA,  Gas Limit for Layer 1.
        /// </summary>
        [JsonProperty(PropertyName = "gasLimitL1")]
        public HexBigInteger GasLimitL1 { get; set; }
    }
}