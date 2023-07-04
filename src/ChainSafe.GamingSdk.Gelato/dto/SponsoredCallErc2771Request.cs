using Newtonsoft.Json;

namespace ChainSafe.GamingSdk.Gelato.Dto
{
    public class SponsoredCallErc2771Request : Erc2771UserParams
    {
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