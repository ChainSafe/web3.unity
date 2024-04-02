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
        [EnumMember(Value = "erc20")]
        Erc20,
        [EnumMember(Value = "erc721")]
        Erc721,
        [EnumMember(Value = "erc1155")]
        Erc1155,
    }

    public class Handler
    {
        [JsonProperty(PropertyName = "type")]
        public ResourceType Type { get; set; }

        [JsonProperty(PropertyName = "address")]
        public string Address { get; set; }
    }
}