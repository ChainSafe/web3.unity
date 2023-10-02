using Newtonsoft.Json;

namespace ChainSafe.Gaming.WalletConnect
{
    public struct WalletLink
    {
        [JsonProperty("native")]
        public string NativeProtocol;

        [JsonProperty("universal")]
        public string UniversalUrl;
    }
}