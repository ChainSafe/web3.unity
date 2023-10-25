using Nethereum.Hex.HexTypes;
using Newtonsoft.Json;

namespace ChainSafe.GamingSdk.Gelato.Types
{
    /// <summary>
    /// Represents contract-related information, including endpoints and addresses, used in the context of Ethereum smart contracts.
    /// </summary>
    public class Contract
    {
        /// <summary>
        ///    DATA, 20 Bytes - The ERC2771 relayer endpoint.
        /// </summary>
        [JsonProperty(PropertyName = "relayERC2771")]
        public HexBigInteger RelayErc2771 { get; set; }

        /// <summary>
        ///    DATA, 20 Bytes - The 1balance address for the targer relayer.
        /// </summary>
        [JsonProperty(PropertyName = "relay1BalanceERC2771")]
        public HexBigInteger Relay1BalanceErc2771 { get; set; }

        /// <summary>
        ///    DATA, 20 Bytes - The zkSync relayer endpoint.
        /// </summary>
        [JsonProperty(PropertyName = "relayERC2771zkSync")]
        public HexBigInteger RelayErc2771ZkSync { get; set; }

        /// <summary>
        ///    DATA, 20 Bytes - The zkSync 1Balance Relayer endpoint.
        /// </summary>
        [JsonProperty(PropertyName = "relay1BalanceERC2771zkSync")]
        public HexBigInteger Relay1BalanceErc2771ZkSync { get; set; }
    }
}