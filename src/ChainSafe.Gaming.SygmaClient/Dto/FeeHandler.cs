using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace ChainSafe.Gaming.SygmaClient.Dto
{
    public enum FeeHandlerType
    {
        [EnumMember(Value = "dynamic")]
        Dynamic,
        [EnumMember(Value = "basic")]
        Basic,
        [EnumMember(Value = "percentage")]
        Percentage,
    }

    public class FeeHandler
    {
        [JsonProperty(PropertyName = "type")]
        public FeeHandlerType Type { get; set; }

        [JsonProperty(PropertyName = "address")]
        public string Address { get; set; }
    }
}