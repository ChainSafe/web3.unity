using System.Web;
using ChainSafe.Gaming.WalletConnect.Models;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Environment;

namespace ChainSafe.Gaming.WalletConnect
{
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

        // Redirect for connection using the pre-selected wallet
        public void RedirectConnection(string connectionUri, string walletName) // todo check what happens when calling on Android
        {
            var walletData = walletRegistry.GetWallet(walletName);
            var deeplink = BuildConnectionDeeplink(walletData, connectionUri);
            osMediator.OpenUrl(deeplink);
        }

        // Delegate redirection for connection to the OS
        public void RedirectConnectionOsManaged(string connectionUri)
        {
            osMediator.OpenUrl(connectionUri);
        }

        // Redirect to the pre-selected wallet
        public void Redirect(string walletName)
        {
            var walletData = walletRegistry.GetWallet(walletName);
            Redirect(walletData);
        }

        // Redirect to the pre-selected wallet
        public void Redirect(WalletConnectWalletModel walletData)
        {
            var deepLink = GetRedirectionDeeplink(walletData);
            osMediator.OpenUrl(deepLink);
        }

        public string GetRedirectionDeeplink(WalletConnectWalletModel walletData)
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

        public string BuildConnectionDeeplink(WalletConnectWalletModel walletData, string connectionUri)
        {
            var linkData = GetPlatformLinkData(walletData);
            var platformUrl = BuildPlatformUrl();

            var finalUrl = $"{platformUrl}wc?uri={HttpUtility.UrlEncode(connectionUri)}";

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
                    throw new Web3Exception(
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

        private WalletLinkModel GetPlatformLinkData(WalletConnectWalletModel walletData)
        {
            return osMediator.IsMobilePlatform ? walletData.Mobile : walletData.Desktop;
        }
    }
}