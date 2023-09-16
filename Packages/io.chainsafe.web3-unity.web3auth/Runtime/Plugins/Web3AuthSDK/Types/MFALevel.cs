using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

[JsonConverter(typeof(StringEnumConverter))]
public enum MFALevel
{
    [EnumMember(Value = "default")]
    DEFAULT,
    [EnumMember(Value = "optional")]
    OPTIONAL,
    [EnumMember(Value = "mandatory")]
    MANDATORY,
    [EnumMember(Value = "none")]
    NONE
}
