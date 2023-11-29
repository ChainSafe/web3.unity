using System;
#if UNITY_WEBGL
using System.Runtime.InteropServices;
#endif
using MetaMask.Models;
using UnityEngine;

namespace MetaMask.Transports.Unity
{

    public abstract class MetaMaskUnityScriptableObjectTransport : ScriptableObject, IMetaMaskTransport
    {
        public bool isWebGL
        {
            get
            {
#if UNITY_WEBGL
                return true;
#else
                return false;

#endif
            }
        }
        public abstract event EventHandler<MetaMaskUnityRequestEventArgs> Requesting;

        public abstract void Initialize();

        public abstract void UpdateUrls(string universalLink, string deepLink);

        public abstract void OnConnectRequest();

        public abstract void OnFailure(Exception error);

        public abstract void OnRequest(string id, MetaMaskEthereumRequest request);
        public abstract void OnOTPCode(int code);

        public abstract void OnSessionRequest(MetaMaskSessionData session);

        public abstract void OnSuccess();
        
#if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")]
        public static extern void OpenMetaMaskDeeplink(string url);
        
        [DllImport("__Internal")]
        public static extern bool WebGLIsMobile();
#endif

        public bool IsMobile
        {
            get
            {
                #if UNITY_WEBGL && !UNITY_EDITOR
                    return WebGLIsMobile();
                #else
                    return Application.isMobilePlatform;
                #endif
            }
        }
        public abstract void OnDisconnect();

        protected void OpenDeeplinkURL(string url)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            OpenMetaMaskDeeplink(url);
#else
            Application.OpenURL(url);
#endif
        }

    }

    public class MetaMaskUnityRequestEventArgs : EventArgs
    {

        /// <summary>The request to be sent to MetaMask.</summary>
        public readonly MetaMaskEthereumRequest Request;

        /// <summary>Initializes a new instance of the <see cref="MetaMaskUnityRequestEventArgs"/> class.</summary>
        /// <param name="request">The request.</param>
        public MetaMaskUnityRequestEventArgs(MetaMaskEthereumRequest request)
        {
            this.Request = request;
        }

    }

}