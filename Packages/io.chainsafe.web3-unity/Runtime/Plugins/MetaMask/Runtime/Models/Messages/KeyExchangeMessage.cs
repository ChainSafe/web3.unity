using System.Text.Json.Serialization;

using Newtonsoft.Json;

namespace MetaMask.Models
{

    /// <summary>
    /// The MetaMask key exchange message.
    /// </summary>
    public class KeyExchangeMessage : TypedMessage
    {

        [JsonProperty("pubkey")]
        [JsonPropertyName("pubkey")]
        public string PublicKey { get; set; }

        public KeyExchangeMessage()
        {
        }

        public KeyExchangeMessage(string type)
        {
            Type = type;
        }

        public KeyExchangeMessage(string type, string publicKey)
        {
            Type = type;
            PublicKey = publicKey;
        }

    }
}
