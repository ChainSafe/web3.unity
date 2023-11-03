using Newtonsoft.Json;

namespace ChainSafe.Gaming.WalletConnect.Models
{
    /// <summary>
    /// Wallet linking model used for opening/redirecting to wallets using a deeplink from either a Native or a Universal/http protocol.
    /// </summary>
    public struct WalletLinkModel
    {
        /// <summary>
        /// Native protocol deeplink for redirecting to wallet.
        /// If available this redirects without opening a browser.
        /// </summary>
        [JsonProperty("native")]
        public string NativeProtocol { get; private set; }

        /// <summary>
        /// Universal url deeplink for redirecting to wallet.
        /// If available this opens a browser that triggers a native deeplink.
        /// </summary>
        [JsonProperty("universal")]
        public string UniversalUrl { get; private set; }
    }
}