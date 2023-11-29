using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace MetaMask.Models
{
    public class OtpAnswerMessage : TypedMessage
    {
        [JsonProperty("otpAnswer")]
        [JsonPropertyName("otpAnswer")]
        public int OtpAnswer { get; set; }
    }
}