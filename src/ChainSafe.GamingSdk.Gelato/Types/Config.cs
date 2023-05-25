using Nethereum.Hex.HexTypes;
using Newtonsoft.Json;

namespace ChainSafe.GamingSdk.Gelato.Types
{
    public class Config
    {
        /// <summary>
        ///    DATA - Relayer endpoint
        /// </summary>
        [JsonProperty(PropertyName = "chainId")]
        public long ChainId { get; set; }

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
        ///    DATA, 20 Bytes - The ERC2771 relayer endpoint
        /// </summary>
        [JsonProperty(PropertyName = "relayERC2771")]
        public HexBigInteger RelayERC2771 { get; set; }

        /// <summary>
        ///    DATA, 20 Bytes - The 1balance address for the targer relayer
        /// </summary>
        [JsonProperty(PropertyName = "relay1BalanceERC2771")]
        public HexBigInteger Relay1BalanceERC2771 { get; set; }

        /// <summary>
        ///    DATA, 20 Bytes - The zkSync relayer endpoint
        /// </summary>
        [JsonProperty(PropertyName = "relayERC2771zkSync")]
        public HexBigInteger RelayERC2771zkSync { get; set; }

        /// <summary>
        ///    DATA, 20 Bytes - The zkSync 1Balance Relayer endpoint
        /// </summary>
        [JsonProperty(PropertyName = "relay1BalanceERC2771zkSync")]
        public HexBigInteger Relay1BalanceERC2771zkSync { get; set; }
    }
}