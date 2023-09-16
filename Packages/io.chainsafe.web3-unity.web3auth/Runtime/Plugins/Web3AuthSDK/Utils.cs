using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using UnityEngine;

public static class Utils
{
#if !UNITY_EDITOR && UNITY_IOS
    [DllImport("__Internal")]
    extern static void web3auth_launch(string url, string redirectUri, string objectName);
#endif

#if !UNITY_EDITOR && UNITY_WEBGL
    [DllImport("__Internal")]
    public extern static string GetCurrentURL();

    [DllImport("__Internal")]
    extern static void OpenURL(string url);

    [DllImport("__Internal")]
    public extern static void RemoveAuthCodeFromURL();
#endif


    public static void LaunchUrl(string url, string redirectUri = null, string objectName = null)
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        Application.OpenURL(url);
#elif UNITY_ANDROID
        using (var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        using (var activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
        using (var browserView = new AndroidJavaObject("com.web3auth.unity.android.BrowserView"))
        {
            browserView.CallStatic("launchUrl", activity, url);
        }

#elif UNITY_IOS
    var uri = new Uri(redirectUri);
    web3auth_launch(url, uri.Scheme, objectName);
#elif UNITY_WEBGL
    OpenURL(url);
#endif
    }


    public static byte[] DecodeBase64(string text)
    {
        var output = text;
        output = output.Replace('-', '+');
        output = output.Replace('_', '/');
        switch (output.Length % 4)
        {
            case 0: break;
            case 2: output += "=="; break;
            case 3: output += "="; break;
            default: throw new FormatException(text);
        }
        var converted = Convert.FromBase64String(output);
        return converted;
    }

    public static Dictionary<string, string> ParseQuery(string text)
    {
        if (text.Length > 0 && text[0] == '?')
            text = text.Remove(0, 1);

        var parts = text.Split('&').Where(x => !string.IsNullOrEmpty(x)).ToList();

        Dictionary<string, string> result = new Dictionary<string, string>();

        if (parts.Count > 0)
        {
            result = parts.ToDictionary(
                c => c.Split('=')[0],
                c => Uri.UnescapeDataString(c.Split('=')[1])
            );
        }

        return result;
    }

    public static int GetRandomUnusedPort()
    {
        var listener = new TcpListener(IPAddress.Any, 0);
        listener.Start();
        var port = ((IPEndPoint)listener.LocalEndpoint).Port;
        listener.Stop();
        return port;
    }


}