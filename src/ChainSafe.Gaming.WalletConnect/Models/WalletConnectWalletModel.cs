using System;
using Newtonsoft.Json;
using UnityEngine;
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

        public void OpenDeeplink(ConnectedData data, bool useNative = false)
        {
            string uri = string.Empty;

#if UNITY_ANDROID
            uri = data.Uri; // Android OS should handle wc: protocol
#elif UNITY_IOS

            // on iOS, we need to use one of the wallet links
            WalletLink linkData = Application.isMobilePlatform ? Mobile : Desktop;

            var universalUrl = useNative ? linkData.NativeProtocol : linkData.UniversalUrl;

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
#endif
            WCLogger.Log($"Opening URL {uri}");

            Application.OpenURL(uri);
        }

        public void OpenWallet()
        {
            WalletLinkModel linkData = Application.isMobilePlatform ? Mobile : Desktop;

            string universalUrl = linkData.UniversalUrl;

            if (string.IsNullOrWhiteSpace(universalUrl))
            {
                throw new Exception("Got empty URI when attempting to create WC deeplink");
            }

            Application.OpenURL(universalUrl);
        }
    }
}