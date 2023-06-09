using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

[JsonConverter(typeof(StringEnumConverter))]
public enum Prompt
{
    [EnumMember(Value = "none")]
    NONE,
    [EnumMember(Value = "login")]
    LOGIN,
    [EnumMember(Value = "consent")]
    CONSENT,
    [EnumMember(Value = "select_account")]
    SELECT_ACCOUNT
}

