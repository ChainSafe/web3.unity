using Nethereum.Hex.HexTypes;
using Newtonsoft.Json;

namespace ChainSafe.GamingSdk.Gelato.Types
{
    public class BaseRelayParams
    {
        /// <summary>
        ///    QUANTITY - The transaction chain id.
        /// </summary>
        [JsonProperty(PropertyName = "chainId")]
        public HexBigInteger ChainId { get; set; }

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
    }
}