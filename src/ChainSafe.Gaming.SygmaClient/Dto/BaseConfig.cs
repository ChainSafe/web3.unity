using System.Collections.Generic;
using System.Runtime.Serialization;
using ChainSafe.Gaming.SygmaClient.Types;
using Nethereum.Hex.HexTypes;
using Newtonsoft.Json;

namespace ChainSafe.Gaming.SygmaClient.Dto
{
    public enum Network
    {
        [EnumMember(Value = "evm")]
        Evm,
        [EnumMember(Value = "substarte")]
        Substrate,
    }

    public class BaseConfig : Domain
    {
        [JsonProperty(PropertyName = "bridge")]
        public string Bridge { get; set; }

        [JsonProperty(PropertyName = "nativeTokenSymbol")]
        public string NativeTokenSymbol { get; set; }

        [JsonProperty(PropertyName = "nativeTokenFullName")]
        public string NativeTokenFullName { get; set; }

        [JsonProperty(PropertyName = "nativeTokenDecimals")]
        public HexBigInteger NativeTokenDecimals { get; set; }

        [JsonProperty(PropertyName = "startBlock")]
        public HexBigInteger StartBlock { get; set; }

        [JsonProperty(PropertyName = "blockConfirmations")]
        public int BlockConfirmations { get; set; }

        [JsonProperty(PropertyName = "resources")]
        public List<EvmResource> Resources { get; set; }
    }

    public class EvmConfig : BaseConfig
    {
        [JsonProperty(PropertyName = "handlers")]
        public List<Handler> Handlers { get; set; }

        [JsonProperty(PropertyName = "feeRouter")]
        public string FeeRouter { get; set; }

        [JsonProperty(PropertyName = "feeHandlers")]
        public List<FeeHandler> FeeHandlers { get; set; }
    }
}