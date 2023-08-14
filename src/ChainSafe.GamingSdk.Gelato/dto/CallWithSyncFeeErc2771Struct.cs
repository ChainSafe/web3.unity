using Nethereum.ABI.FunctionEncoding.Attributes;
using Newtonsoft.Json;

namespace ChainSafe.GamingSdk.Gelato.Dto
{
    [Struct("CallWithSyncFeeERC2771")]
    public class CallWithSyncFeeErc2771Struct : IErc2771StructTypes
    {
        [Parameter("string", "chainId", 1)]
        [JsonProperty(PropertyName = "chainId")]
        public virtual string ChainId { get; set; }

        [Parameter("string", "target", 2)]
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
        public virtual string UserNonce { get; set; }

        [Parameter("uint256", "userDeadline", 6)]
        [JsonProperty(PropertyName = "userDeadline")]
        public virtual string UserDeadline { get; set; }
    }
}