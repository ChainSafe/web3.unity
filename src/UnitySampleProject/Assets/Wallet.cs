using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using WalletConnectSharp.Sign.Models.Engine;

[Serializable]
public struct Wallet
{
    [JsonProperty("mobile")] public WalletLink Mobile;

    [JsonProperty("desktop")] public WalletLink Desktop;

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
                uri = $"{universalUrl}//{uri}";

            else if (universalUrl.EndsWith("/"))
                uri = $"{universalUrl}{uri}";

            else
                uri = $"{universalUrl}/{uri}";
        }

        if (string.IsNullOrWhiteSpace(uri)) throw new Exception("Got empty URI when attempting to create WC deeplink");
#endif
        
        Debug.Log($"Opening URL {uri}");

        Application.OpenURL(uri);
    }

    public void OpenWallet()
    {
        WalletLink linkData = Application.isMobilePlatform ? Mobile : Desktop;

        var universalUrl = linkData.UniversalUrl;

        if (string.IsNullOrWhiteSpace(universalUrl))
            throw new Exception("Got empty URI when attempting to create WC deeplink");

        Application.OpenURL(universalUrl);
    }
}

public struct WalletLink
{
    [JsonProperty("native")]
    public string NativeProtocol;

    [JsonProperty("universal")]
    public string UniversalUrl;
}