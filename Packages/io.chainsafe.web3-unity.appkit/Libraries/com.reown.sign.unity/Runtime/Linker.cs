using System;
using System.Collections.Generic;
using Reown.Core.Common.Logging;
using Reown.Core.Models.Publisher;
using Reown.Sign.Interfaces;
using Reown.Sign.Models;
using Reown.Sign.Unity.Utils;
using UnityEngine;

namespace Reown.Sign.Unity
{
    public class Linker : IDisposable
    {
        private readonly ISignClient _signClient;
        private readonly Dictionary<string, uint> _sessionMessagesCounter = new();

        protected bool disposed;

        public Linker(ISignClient signClient)
        {
            _signClient = signClient;

            RegisterEventListeners();
        }

        private void RegisterEventListeners()
        {
            _signClient.CoreClient.Relayer.Publisher.OnPublishedMessage += OnPublisherPublishedMessage;
        }

        public static void OpenSessionProposalDeepLink(string uri, string nativeRedirect)
        {
            if (string.IsNullOrWhiteSpace(uri))
                throw new ArgumentException("[Linker] Uri cannot be empty.");

#if UNITY_EDITOR && (UNITY_IOS || UNITY_ANDROID)
            // In editor we cannot open _mobile_ deep links, so we just log the uri
            Debug.Log($"[Linker] Requested to open mobile deep link. The uri: {uri}");
#else

            if (string.IsNullOrWhiteSpace(nativeRedirect))
                throw new Exception(
                    $"[Linker] No link found for {Application.platform} platform.");

            var url = BuildConnectionDeepLink(nativeRedirect, uri);

            ReownLogger.Log($"[Linker] Opening URL {url}");

            Application.OpenURL(url);
#endif
        }

        public static void OpenSessionRequestDeepLink(in SessionStruct session)
        {
            if (string.IsNullOrWhiteSpace(session.Topic))
                throw new Exception("[Linker] No session topic found in provided session. Cannot open deep link.");

            if (session.Peer.Metadata == null)
                return;

            var redirectNative = session.Peer.Metadata.Redirect?.Native;

            if (string.IsNullOrWhiteSpace(redirectNative))
            {
                if (!TryGetRecentWalletDeepLink(out var deeplink))
                    return;

                Debug.LogWarning(
                    $"[Linker] No redirect found for {session.Peer.Metadata.Name}. Using deep link from the Recent Wallet."
                );

                if (!deeplink.EndsWith("://"))
                    deeplink = $"{deeplink}://";

                Application.OpenURL(deeplink);
            }
            else
            {
                ReownLogger.Log($"[Linker] Open native deep link: {redirectNative}");

                if (!redirectNative.EndsWith("://"))
                    redirectNative = $"{redirectNative}://";

                Application.OpenURL(redirectNative);
            }
        }

        public static string BuildConnectionDeepLink(string appLink, string wcUri)
        {
            if (string.IsNullOrWhiteSpace(wcUri))
                throw new ArgumentException("[Linker] Uri cannot be empty.");

            if (string.IsNullOrWhiteSpace(appLink))
                throw new ArgumentException("[Linker] Native link cannot be empty.");

            var safeAppUrl = appLink;
            if (!safeAppUrl.Contains("://"))
            {
                safeAppUrl = safeAppUrl.Replace("/", "").Replace(":", "");
                safeAppUrl = $"{safeAppUrl}://";
            }

            if (!safeAppUrl.EndsWith('/'))
                safeAppUrl = $"{safeAppUrl}/";

            var encodedWcUrl = Uri.EscapeDataString(wcUri);

            return $"{safeAppUrl}wc?uri={encodedWcUrl}";
        }

        public void OpenSessionRequestDeepLink(string sessionTopic)
        {
            var session = _signClient.Session.Get(sessionTopic);
            OpenSessionRequestDeepLink(in session);
        }

        public virtual void OpenSessionRequestDeepLink()
        {
            var session = _signClient.AddressProvider.DefaultSession;
            OpenSessionRequestDeepLink(in session);
        }

        protected virtual void OnPublisherPublishedMessage(object sender, PublishParams publishParams)
        {
            UnitySyncContext.Context.Post(_ =>
            {
                if (string.IsNullOrWhiteSpace(publishParams.Topic))
                    return;

                if (_sessionMessagesCounter.TryGetValue(publishParams.Topic, out var messageCount))
                {
                    ReownLogger.Log($"[Linker] OnPublisherPublishedMessage. Message count: {messageCount}");
                    if (messageCount != 0)
                    {
                        _sessionMessagesCounter[publishParams.Topic] = messageCount - 1;
                        OpenSessionRequestDeepLink(publishParams.Topic);
                    }
                }
            }, null);
        }

        public void OpenSessionRequestDeepLinkAfterMessageFromSession(string sessionTopic)
        {
            ReownLogger.Log($"[Linker] OpenSessionRequestDeepLinkAfterMessageFromSession. Topic: {sessionTopic}");
            if (_sessionMessagesCounter.TryGetValue(sessionTopic, out var messageCount))
                _sessionMessagesCounter[sessionTopic] = messageCount + 1;
            else
                _sessionMessagesCounter.Add(sessionTopic, 1);
        }

        public static bool CanOpenURL(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return false;

            try
            {
#if !UNITY_EDITOR && UNITY_IOS
                return _CanOpenURL(url);
#elif !UNITY_EDITOR && UNITY_ANDROID 
                using var urlCheckerClass = new AndroidJavaClass("com.reown.sign.unity.Linker");
                using var unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                using var currentContext = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity");
                var result = urlCheckerClass.CallStatic<bool>("canOpenURL", currentContext, url);
                return result;
#endif
            }
            catch (Exception e)
            {
                ReownLogger.LogError($"[Linker] Exception for url {url}: {e.Message}");
            }

            return false;
        }

#if !UNITY_EDITOR && UNITY_IOS
        [System.Runtime.InteropServices.DllImport("__Internal")]
        public static extern bool _CanOpenURL(string url);
#endif

        private static bool TryGetRecentWalletDeepLink(out string deeplink)
        {
            deeplink = null;

            deeplink = PlayerPrefs.GetString("RE_RECENT_WALLET_DEEPLINK");

            if (string.IsNullOrWhiteSpace(deeplink))
                return false;

            return deeplink != null;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed) return;

            if (disposing)
                _signClient.CoreClient.Relayer.Publisher.OnPublishedMessage -= OnPublisherPublishedMessage;

            disposed = true;
        }
    }
}