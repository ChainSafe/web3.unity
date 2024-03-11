using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace ChainSafe.Gaming.SygmaClient.Dto
{
    public enum ResourceType
    {
        [EnumMember(Value = "fungible")]
        Fungible,

        [EnumMember(Value = "nonFungible")]
        NonFungible,

        [EnumMember(Value = "permissionedGeneric")]
        PermissionedGeneric,

        [EnumMember(Value = "permissionlessGeneric")]
        PermissionlessGeneric,
    }

    public class Handler
    {
        [JsonProperty(PropertyName = "type")]
        public ResourceType Type { get; set; }

        [JsonProperty(PropertyName = "address")]
        public string Address { get; set; }
    }
}