using Nethereum.Hex.HexTypes;
using Newtonsoft.Json;

namespace ChainSafe.GamingSdk.Gelato.Relay
{
    public class RelayRequestOptions
    {
        /// <summary>
        ///    QUANTITY -  the gas limit of the relay call. This effectively sets an upper price limit for the relay call.
        /// </summary>
        [JsonProperty(PropertyName = "gasLimit")]
        public HexBigInteger GasLimit { get; set; }

        /// <summary>
        ///     QUANTITY - the number of retries that Gelato should attempt before discarding this relay call.
        /// </summary>
        [JsonProperty(PropertyName = "retries")]
        public uint Retries { get; set; }
    }
}