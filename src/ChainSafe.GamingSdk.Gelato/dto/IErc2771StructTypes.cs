using Nethereum.ABI.FunctionEncoding.Attributes;
using Newtonsoft.Json;

namespace ChainSafe.GamingSdk.Gelato.Dto
{
    public interface IErc2771StructTypes
    {
        [Parameter("string", "chainId", 1)]
        [JsonProperty(PropertyName = "chainId")]
        public string ChainId { get; set; }

        [Parameter("string", "target", 2)]
        [JsonProperty(PropertyName = "target")]
        public string Target { get; set; }

        [Parameter("bytes", "data", 3)]
        [JsonProperty(PropertyName = "data")]
        public string Data { get; set; }

        [Parameter("address", "user", 4)]
        [JsonProperty(PropertyName = "user")]
        public string User { get; set; }

        [Parameter("uint256", "userNonce", 5)]
        [JsonProperty(PropertyName = "userNonce")]
        public string UserNonce { get; set; }

        [Parameter("uint256", "userDeadline", 6)]
        [JsonProperty(PropertyName = "userDeadline")]
        public string UserDeadline { get; set; }
    }
}