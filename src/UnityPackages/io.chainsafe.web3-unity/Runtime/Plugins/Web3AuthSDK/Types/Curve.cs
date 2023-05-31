using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

[JsonConverter(typeof(StringEnumConverter))]
public enum Curve
{
    [EnumMember(Value = "secp256k1")]
    SECP256K1,
    [EnumMember(Value = "ed25519")]
    ED25519
}
