using Newtonsoft.Json;

namespace ChainSafe.GamingSdk.Gelato.Relay
{
    public class PaymentTokensResponse
    {
        /// <summary>
        ///     DATA, list of accepted ERC20 tokens
        /// </summary>
        [JsonProperty(PropertyName = "paymentTokens")]
        public string[] PaymentTokens { get; set; }
    }
}