using ChainSafe.GamingSdk.Gelato.Types;
using Newtonsoft.Json;

namespace ChainSafe.GamingSdk.Gelato.Dto
{
    public class SponsoredCallRequest : RelayRequestOptions
    {
        /// <summary>
        ///    QUANTITY - The transaction chain id.
        /// </summary>
        [JsonProperty(PropertyName = "chainId")]
        public int ChainId { get; set; }

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

        /// <summary>
        ///    DATA - the signature from the sign typed data request.
        /// </summary>
        [JsonProperty(PropertyName = "sponsorApiKey")]
        public string SponsorApiKey { get; set; }
    }
}