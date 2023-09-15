using Newtonsoft.Json;

namespace ChainSafe.GamingSdk.Gelato.Types
{
    public class BaseCallWithSyncFeeParams
    {
        /// <summary>
        ///     DATA, 20 Bytes - the address of the token that is to be used for payment.
        /// </summary>
        [JsonProperty(PropertyName = "feeToken")]
        public string FeeToken { get; set; }

        /// <summary>
        ///     DATA - an optional boolean (default: true ) denoting what data you would prefer appended to the end of the calldata.
        /// </summary>
        [JsonProperty(PropertyName = "isRelayContext")]
        public bool IsRelayContext { get; set; }
    }
}