using Nethereum.Hex.HexTypes;
using Newtonsoft.Json;

namespace ChainSafe.GamingSdk.Gelato.Types
{
    public enum RelayCall
    {
        /// <summary>A non ERC712 based call with sync fee request</summary>
        CallWithSyncFee,

        /// <summary>A ERC712 based call with sync fee request</summary>
        CallWithSyncFeeErc2771,

        /// <summary>A non ERC712 based call sponsored request</summary>
        SponsoredCall,

        /// <summary>A ERC712 based call sponsored request</summary>
        SponsoredCallErc2771,
    }

    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
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
