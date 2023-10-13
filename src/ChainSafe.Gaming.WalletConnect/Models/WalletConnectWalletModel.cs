using System;
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
                    var universalUrl = useNative ? Mobile.NativeProtocol : Mobile.UniversalUrl;

                    uri = data.Uri;

                    if (!string.IsNullOrWhiteSpace(universalUrl))
                    {
                        uri = data.Uri;

                        if (useNative)
                        {
                            uri = $"{universalUrl}//{uri}";
                        }
                        else if (universalUrl.EndsWith("/"))
                        {
                            uri = $"{universalUrl}{uri}";
                        }
                        else
                        {
                            uri = $"{universalUrl}/{uri}";
                        }
                    }

                    if (string.IsNullOrWhiteSpace(uri))
                    {
                        throw new Exception("Got empty URI when attempting to create WC deeplink");
                    }

                    break;

                default:
                    WCLogger.LogError($"{operatingSystemMediator.Platform} Platform doesn't support deeplink");
                    break;
            }

            WCLogger.Log($"Opening URL {uri}");

            operatingSystemMediator.OpenUrl(uri);
        }

        public void OpenWallet(IOperatingSystemMediator operatingSystemMediator)
        {
            WalletLinkModel linkData = operatingSystemMediator.IsMobilePlatform ? Mobile : Desktop;

            string universalUrl = linkData.UniversalUrl;

            if (string.IsNullOrWhiteSpace(universalUrl))
            {
                throw new Exception("Got empty URI when attempting to create WC deeplink");
            }

            operatingSystemMediator.OpenUrl(universalUrl);
        }
    }
}