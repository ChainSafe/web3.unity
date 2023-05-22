using Nethereum.Hex.HexTypes;
using Newtonsoft.Json;

namespace ChainSafe.GamingSdk.Gelato.Types
{
    public class Config
    {
        /// <summary>
        ///    DATA - Relayer endpoint
        /// </summary>
        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }

        /// <summary>
        ///     DATA - Contract settings for Gelato
        /// </summary>
        [JsonProperty(PropertyName = "contract")]
        public Contract Contract { get; set; }
    }

    public class Contract
    {
        /// <summary>
        ///    DATA, 20 Bytes - The address the transaction is being sent to.
        /// </summary>
        [JsonProperty(PropertyName = "relayERC2771")]
        public HexBigInteger RelayERC2771 { get; set; }
    }
}