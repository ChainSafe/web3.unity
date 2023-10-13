using System.Web;
using ChainSafe.Gaming.Web3.Environment;
using Newtonsoft.Json;
using WalletConnectSharp.Common.Logging;
using WalletConnectSharp.Sign.Models.Engine;

namespace ChainSafe.Gaming.WalletConnect.Models
{
    public class WalletConnectWalletModel
    {
        [JsonProperty("name")]
        public string Name { get; private set; }

        [JsonProperty("mobile")]
        public WalletLinkModel Mobile { get; private set; }

        [JsonProperty("desktop")]
        public WalletLinkModel Desktop { get; private set; }

        [JsonProperty("image_url")]
        public ImageUrlsModel Images { get; private set; }

        public void OpenDeeplink(ConnectedData data, IOperatingSystemMediator operatingSystemMediator, string symKey)
        {
            string uri = data.Uri;

            // if storage is persistent sometimes symKey might be embedded in uri incorrectly, wallet connect issue
            string symKeyInUri = $"symKey={symKey}";

            // remove symKey in Uri if it exists
            uri = uri.Replace(symKeyInUri, string.Empty);

            switch (operatingSystemMediator.Platform)
            {
                case Platform.Android:
                    // Android OS should handle wc: protocol
                    break;

                case Platform.IOS: case Platform.Desktop: case Platform.Editor:
                    if (string.IsNullOrEmpty(symKey))
                    {
                        WCLogger.LogError($"Failed to open {Name} Wallet Deeplink : SymKey NullOrEmpty");

                        return;
                    }

                    uri = GetDeeplink(data.Uri, operatingSystemMediator.IsMobilePlatform, symKey);
                    break;

                default:
                    WCLogger.LogError($"{operatingSystemMediator.Platform} Platform doesn't support {Name} Wallet deeplink");
                    return;
            }

            if (string.IsNullOrEmpty(uri))
            {
                WCLogger.LogError($"Failed to open {Name} Wallet Deeplink : Uri NullOrEmpty");

                return;
            }

            WCLogger.Log($"Opening URL {uri}");

            operatingSystemMediator.OpenUrl(uri);
        }

        // Deeplink Building
        private string GetDeeplink(string uri, bool isMobilePlatform, string symKey)
        {
            WalletLinkModel linkData = GetLinkData(isMobilePlatform);

            // prefer native protocol
            return CanUseNativeProtocol(isMobilePlatform) ? BuildNativeDeeplink(linkData.NativeProtocol, uri, symKey) : BuildUniversalDeeplink(linkData.UniversalUrl, uri, symKey);
        }

        private string BuildNativeDeeplink(string url, string uri, string symKey)
        {
            if (url.EndsWith(':'))
            {
                url += "//";
            }

            url += "wc";

            return AddDeeplinkParams(url, uri, symKey);
        }

        private string BuildUniversalDeeplink(string url, string uri, string symKey)
        {
            if (!url.EndsWith('/'))
            {
                url += "/";
            }

            return AddDeeplinkParams(url, uri, symKey);
        }

        private string AddDeeplinkParams(string url, string uri, string symKey)
        {
            url += $"?uri={HttpUtility.UrlEncode(uri)}";

            url += $"&symKey={HttpUtility.UrlEncode(symKey)}";

            return url;
        }

        public void OpenWallet(IOperatingSystemMediator operatingSystemMediator)
        {
            bool isMobilePlatform = operatingSystemMediator.IsMobilePlatform;

            WalletLinkModel linkData = GetLinkData(isMobilePlatform);

            string deeplink = CanUseNativeProtocol(isMobilePlatform) ? linkData.NativeProtocol : linkData.UniversalUrl;

            if (string.IsNullOrEmpty(deeplink))
            {
                WCLogger.LogError($"Failed to open {Name} Wallet : NullOrEmpty Deeplink URI for {operatingSystemMediator.Platform} Platform");

                return;
            }

            operatingSystemMediator.OpenUrl(deeplink);
        }

        private WalletLinkModel GetLinkData(bool isMobilePlatform)
        {
            return isMobilePlatform ? Mobile : Desktop;
        }

        private bool CanUseNativeProtocol(bool isMobilePlatform)
        {
            string nativeUrl = GetLinkData(isMobilePlatform).NativeProtocol;

            return !string.IsNullOrWhiteSpace(nativeUrl) && nativeUrl != ":";
        }
    }
}