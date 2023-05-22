using Nethereum.Hex.HexTypes;
using Newtonsoft.Json;

namespace ChainSafe.GamingSdk.Gelato.Types
{
    public enum RelayCall {
        CallWithSyncFee,
        CallWithSyncFeeERC2771,
        SponsoredCall,
        SponsoredCallERC2771,
    }

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

    public class ApiKey 
    {
        /// <summary>
        ///     DATA - api key of the 1Balance account that is sponsoring the transaction
        /// </summary>
        [JsonProperty(PropertyName = "sponsorApiKey")]
        public string SponsorApiKey;
    }
}
