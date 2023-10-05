using System;
using Newtonsoft.Json;
using UnityEngine;
using WalletConnectSharp.Common.Logging;
using WalletConnectSharp.Sign.Models.Engine;

namespace ChainSafe.Gaming.WalletConnect
{
    public class WalletConnectWalletModel
    {
        [JsonProperty("mobile")]
        public WalletLink Mobile { get; private set; }

        [JsonProperty("desktop")]
        public WalletLink Desktop { get; private set; }

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
            WalletLink linkData = Application.isMobilePlatform ? Mobile : Desktop;

            string universalUrl = linkData.UniversalUrl;

            if (string.IsNullOrWhiteSpace(universalUrl))
            {
                throw new Exception("Got empty URI when attempting to create WC deeplink");
            }

            Application.OpenURL(universalUrl);
        }
    }
}