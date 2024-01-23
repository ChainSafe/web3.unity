using System.Web;
using ChainSafe.Gaming.WalletConnect.Models;
using ChainSafe.Gaming.WalletConnect.Wallets;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Environment;

namespace ChainSafe.Gaming.WalletConnect
{
    /// <summary>
    /// Component responsible for redirection of a user to a wallet app.
    /// </summary>
    public class RedirectionHandler
    {
        private readonly IWalletRegistry walletRegistry;
        private readonly IOperatingSystemMediator osMediator;

        public RedirectionHandler(IWalletRegistry walletRegistry, IOperatingSystemMediator osMediator)
        {
            this.osMediator = osMediator;
            this.walletRegistry = walletRegistry;
        }

        private static bool NativeProtocolAvailable(WalletLinkModel linkData)
        {
            return !string.IsNullOrWhiteSpace(linkData.NativeProtocol) && linkData.NativeProtocol != ":";
        }

        private static bool UniversalProtocolAvailable(WalletLinkModel linkData)
        {
            return !string.IsNullOrWhiteSpace(linkData.UniversalUrl);
        }

        /// <summary>
        /// Redirect for connection using the pre-selected wallet.
        /// </summary>
        /// <param name="connectionUri">The connection URI provided by WalletConnect.</param>
        /// <param name="walletName">The name of the wallet to redirect user to.</param>
        public void RedirectConnection(string connectionUri, string walletName)
        {
            var walletData = walletRegistry.GetWallet(walletName);
            var deeplink = BuildConnectionDeeplink(walletData, connectionUri);
            osMediator.OpenUrl(deeplink);
        }

        /// <summary>
        /// Delegate redirection for connection to the OS.
        /// </summary>
        /// <param name="connectionUri">The connection URI provided by WalletConnect.</param>
        public void RedirectConnectionOsManaged(string connectionUri)
        {
            osMediator.OpenUrl(connectionUri);
        }

        /// <summary>
        /// Redirect to the pre-selected wallet using the wallet name.
        /// </summary>
        /// <param name="connectionUri">The connection URI provided by WalletConnect.</param>
        public void Redirect(string walletName)
        {
            var walletData = walletRegistry.GetWallet(walletName);
            Redirect(walletData);
        }

        /// <summary>
        /// Redirect to the pre-selected wallet.
        /// </summary>
        /// <param name="connectionUri">The connection URI provided by WalletConnect.</param>
        public void Redirect(WalletModel walletData)
        {
            var deepLink = GetRedirectionDeeplink(walletData);
            osMediator.OpenUrl(deepLink);
        }

        /// <summary>
        /// Used to get a redirection deeplink for the <see cref="WalletModel"/>.
        /// </summary>
        /// <param name="walletData">Data of the wallet that we want to redirect to.</param>
        /// <returns>Deeplink for redirection.</returns>
        public string GetRedirectionDeeplink(WalletModel walletData)
        {
            var linkData = GetPlatformLinkData(walletData);
            var deeplink = GetPlatformDeeplink();
            return deeplink;

            string GetPlatformDeeplink()
            {
                if (osMediator.Platform != Platform.IOS)
                {
                    return NativeProtocolAvailable(linkData) ? linkData.NativeProtocol : linkData.UniversalUrl;
                }
                else
                {
                    return UniversalProtocolAvailable(linkData) ? linkData.UniversalUrl : linkData.NativeProtocol;
                }
            }
        }

        /// <summary>
        /// Used to get a connection deeplink for the particular wallet.
        /// </summary>
        /// <param name="walletData">Data of the wallet that we want to connect.</param>
        /// <param name="connectionUri">The connection URI provided by WalletConnect.</param>
        /// <returns>Deeplink for connection.</returns>
        /// <exception cref="WalletConnectException">Invalid format of deeplink registered for the provided wallet.</exception>
        public string BuildConnectionDeeplink(WalletModel walletData, string connectionUri)
        {
            var linkData = GetPlatformLinkData(walletData);
            var platformUrl = BuildPlatformUrl();

            var finalUrl = $"{platformUrl}wc?uri={HttpUtility.UrlEncode(connectionUri)}"; // use System.Uri.EscapeDataString(uri) if this doesn't work

            return finalUrl;

            string BuildPlatformUrl()
            {
                if (osMediator.Platform != Platform.IOS)
                {
                    return NativeProtocolAvailable(linkData) ? BuildNativeUrl() : BuildUniversalUrl();
                }
                else
                {
                    return UniversalProtocolAvailable(linkData) ? BuildUniversalUrl() : BuildNativeUrl();
                }
            }

            string BuildNativeUrl()
            {
                var nativeUrl = linkData.NativeProtocol;

                if (!nativeUrl.Contains(':'))
                {
                    throw new WalletConnectException(
                        $"Native protocol deeplink for {walletData.Name} had incorrect format: no \":\" symbol.");
                }

                var pureProtocol = nativeUrl[.. (nativeUrl.IndexOf(':') + 1)];
                return $"{pureProtocol}//";
            }

            string BuildUniversalUrl()
            {
                var universalUrl = linkData.UniversalUrl;

                if (!universalUrl.EndsWith('/'))
                {
                    universalUrl += '/';
                }

                return universalUrl;
            }
        }

        private WalletLinkModel GetPlatformLinkData(WalletModel walletData)
        {
            return osMediator.IsMobilePlatform ? walletData.Mobile : walletData.Desktop;
        }
    }
}