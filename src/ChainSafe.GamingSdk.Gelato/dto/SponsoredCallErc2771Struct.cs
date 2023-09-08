using System.Numerics;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Newtonsoft.Json;

namespace ChainSafe.GamingSdk.Gelato.Dto
{
    [Struct("SponsoredCallERC2771")]
    public class SponsoredCallErc2771Struct : IErc2771StructTypes
    {
        [Parameter("uint256", "chainId", 1)]
        [JsonProperty(PropertyName = "chainId")]
        public virtual BigInteger? ChainId { get; set; }

        [Parameter("address", "target", 2)]
        [JsonProperty(PropertyName = "target")]
        public virtual string Target { get; set; }

        [Parameter("bytes", "data", 3)]
        [JsonProperty(PropertyName = "data")]
        public virtual string Data { get; set; }

        [Parameter("address", "user", 4)]
        [JsonProperty(PropertyName = "user")]
        public virtual string User { get; set; }

        [Parameter("uint256", "userNonce", 5)]
        [JsonProperty(PropertyName = "userNonce")]
        public virtual BigInteger? UserNonce { get; set; }

        [Parameter("uint256", "userDeadline", 6)]
        [JsonProperty(PropertyName = "userDeadline")]
        public virtual BigInteger? UserDeadline { get; set; }
    }
}