using Nethereum.Hex.HexTypes;
using Newtonsoft.Json;

namespace ChainSafe.GamingSdk.Gelato.Dto
{
    public class EstimatedFeeResponse
    {
        /// <summary>
        ///     Quantity, estimated cost of transact.
        /// </summary>
        [JsonProperty(PropertyName = "estimatedFee")]
        public HexBigInteger EstimatedFee { get; set; }
    }
}