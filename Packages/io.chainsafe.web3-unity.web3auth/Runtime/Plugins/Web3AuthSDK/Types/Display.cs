using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

[JsonConverter(typeof(StringEnumConverter))]
public enum Display
{
    [EnumMember(Value = "page")]
    PAGE,
    [EnumMember(Value = "popup")]
    POPUP,
    [EnumMember(Value = "touch")]
    TOUCH,
    [EnumMember(Value = "wap")]
    WAP
}
