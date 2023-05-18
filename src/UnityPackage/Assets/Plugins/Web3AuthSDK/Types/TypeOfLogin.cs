using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

[JsonConverter(typeof(StringEnumConverter))]
public enum TypeOfLogin
{
    [EnumMember(Value = "google")]
    GOOGLE,
    [EnumMember(Value = "facebook")]
    FACEBOOK,
    [EnumMember(Value = "reddit")]
    REDDIT,
    [EnumMember(Value = "discord")]
    DISCORD,
    [EnumMember(Value = "twitch")]
    TWITCH,
    [EnumMember(Value = "apple")]
    APPLE,
    [EnumMember(Value = "line")]
    LINE,
    [EnumMember(Value = "github")]
    GITHUB,
    [EnumMember(Value = "kakao")]
    KAKAO,
    [EnumMember(Value = "linkedin")]
    LINKEDIN,
    [EnumMember(Value = "twitter")]
    TWITTER,
    [EnumMember(Value = "weibo")]
    WEIBO,
    [EnumMember(Value = "wechat")]
    WECHAT,
    [EnumMember(Value = "email_passwordless")]
    EMAIL_PASSWORDLESS,
    [EnumMember(Value = "email_password")]
    EMAIL_PASSWORD,
    [EnumMember(Value = "jwt")]
    JWT
}