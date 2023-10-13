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

        public void OpenDeeplink(ConnectedData data, IOperatingSystemMediator operatingSystemMediator, bool useNative = false)
        {
            string uri = string.Empty;

            switch (operatingSystemMediator.Platform)
            {
                case Platform.Android:
                    uri = data.Uri; // Android OS should handle wc: protocol
                    break;

                case Platform.IOS:
                    uri = GetIOSDeeplink(data.Uri, useNative);
                    break;

                default:
                    WCLogger.LogError($"{operatingSystemMediator.Platform} Platform doesn't support deeplink");
                    break;
            }

            WCLogger.Log($"Opening URL {uri}");

            operatingSystemMediator.OpenUrl(uri);
        }

        private string GetIOSDeeplink(string uri, bool useNative)
        {
            string url = useNative ? Mobile.NativeProtocol : Mobile.UniversalUrl;

            if (!string.IsNullOrWhiteSpace(url))
            {
                if (useNative)
                {
                    uri = $"{url}//{uri}";
                }
                else if (url.EndsWith("/"))
                {
                    uri = $"{url}{uri}";
                }
                else
                {
                    uri = $"{url}/{uri}";
                }
            }

            if (string.IsNullOrEmpty(uri))
            {
                WCLogger.LogError("Failed to open Wallet for IOS: NullOrEmpty URI");
            }

            return uri;
        }

        public void OpenWallet(IOperatingSystemMediator operatingSystemMediator)
        {
            WalletLinkModel linkData = operatingSystemMediator.IsMobilePlatform ? Mobile : Desktop;

            string universalUrl = linkData.UniversalUrl;

            if (string.IsNullOrEmpty(universalUrl))
            {
                WCLogger.LogError($"Failed to open Wallet : NullOrEmpty Deeplink URI for {operatingSystemMediator.Platform} Platform");
            }

            operatingSystemMediator.OpenUrl(universalUrl);
        }
    }
}