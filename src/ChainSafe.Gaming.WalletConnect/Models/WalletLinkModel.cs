using Newtonsoft.Json;

namespace ChainSafe.Gaming.WalletConnect.Models
{
    public struct WalletLinkModel
    {
        [JsonProperty("native")]
        public string NativeProtocol { get; private set; }

        [JsonProperty("universal")]
        public string UniversalUrl { get; private set; }
    }
}