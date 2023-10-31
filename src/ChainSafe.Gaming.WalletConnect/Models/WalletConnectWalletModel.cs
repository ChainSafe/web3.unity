using System;
using System.Web;
using ChainSafe.Gaming.Web3.Environment;
using Newtonsoft.Json;
using WalletConnectSharp.Common.Logging;
using WalletConnectSharp.Sign.Models.Engine;

namespace ChainSafe.Gaming.WalletConnect.Models
{
    /// <summary>
    /// Wallet Connects wallet model used for identifying and redirecting wallets.
    /// </summary>
    public class WalletConnectWalletModel
    {
        /// <summary>
        /// Name of the wallet.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; private set; }

        /// <summary>
        /// <see cref="WalletLinkModel"/> for mobile platforms.
        /// </summary>
        [JsonProperty("mobile")]
        public WalletLinkModel Mobile { get; private set; }

        /// <summary>
        /// <see cref="WalletLinkModel"/> for desktop platforms.
        /// </summary>
        [JsonProperty("desktop")]
        public WalletLinkModel Desktop { get; private set; }

        /// <summary>
        /// wallet icons urls.
        /// </summary>
        [JsonProperty("image_url")]
        public ImageUrlsModel Images { get; private set; }

        /// <summary>
        /// Open Wallet to connect with using Uri from <see cref="ConnectedData"/>.
        /// </summary>
        /// <param name="uri">Uri from <see cref="ConnectedData"/> used for connecting to a wallet.</param>
        /// <param name="operatingSystemMediator">Operating System for current platform.</param>
        public void OpenDeeplink(string uri, IOperatingSystemMediator operatingSystemMediator)
        {
            switch (operatingSystemMediator.Platform)
            {
                case Platform.Android:
                    // Android OS should handle wc: protocol
                    break;

                case Platform.IOS:
                case Platform.Desktop:
                case Platform.Editor:
                    uri = GetDeeplink(uri, operatingSystemMediator.IsMobilePlatform);
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
        private string GetDeeplink(string uri, bool isMobilePlatform)
        {
            WalletLinkModel linkData = GetLinkData(isMobilePlatform);

            // prefer native protocol
            return CanUseNativeProtocol(isMobilePlatform) ? BuildNativeDeeplink(linkData.NativeProtocol, uri) : BuildUniversalDeeplink(linkData.UniversalUrl, uri);
        }

        private string BuildNativeDeeplink(string url, string uri)
        {
            if (url.EndsWith(':'))
            {
                url += "//";
            }
            else if (url.EndsWith("//"))
            {
                url += "wc";
            }

            return AddDeeplinkParams(url, uri);
        }

        private string BuildUniversalDeeplink(string url, string uri)
        {
            return AddDeeplinkParams(url, uri);
        }

        private string AddDeeplinkParams(string url, string uri)
        {
            url += $"?uri={HttpUtility.UrlEncode(uri)}";

            return url;
        }

        /// <summary>
        /// Opens wallet by using deeplink.
        /// </summary>
        /// <param name="operatingSystemMediator">Operating System for current platform.</param>
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

        /// <summary>
        /// Is a wallet available for <see cref="Platform"/>.
        /// </summary>
        /// <param name="platform">Platform to wallet app availability.</param>
        /// <returns>True if wallet is available for <see cref="Platform"/>.</returns>
        public bool IsAvailableForPlatform(Platform platform)
        {
            switch (platform)
            {
                case Platform.Editor:
                case Platform.Desktop:
                    return CanUseNativeProtocol(false) || !string.IsNullOrEmpty(Desktop.UniversalUrl);

                case Platform.Android:
                case Platform.IOS:
                    return CanUseNativeProtocol(true) || !string.IsNullOrEmpty(Mobile.UniversalUrl);

                // currently Wallet Connect doesn't support WebGL.
                default:
                    WCLogger.Log($"Unsupported Platform {platform}");
                    return false;
            }
        }
    }
}