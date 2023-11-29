using System;
using System.Collections.Generic;

using MetaMask.Models;
using MetaMask.SocketIOClient;
using UnityEngine;

namespace MetaMask.Transports.Unity
{

    public class MetaMaskUnityTransportBroadcaster : MonoBehaviour
    {

        #region Fields

        private static MetaMaskUnityTransportBroadcaster instance;


        [SerializeField]
        protected GameObject[] listeners;

        protected List<IMetaMaskUnityTransportListener> allListeners = new List<IMetaMaskUnityTransportListener>();

        #endregion

        #region Properties

        public static MetaMaskUnityTransportBroadcaster Instance
        {
            get
            {
                if (instance == null)
                {
                    var instances = FindObjectsOfType<MetaMaskUnityTransportBroadcaster>();
                    if (instances.Length > 1)
                    {
                        Debug.LogError("There are more than 1 instances of " + nameof(MetaMaskUnityTransportBroadcaster) + " inside the scene, there should be only one.");
                        instance = instances[0];
                    }
                    else
                    {
                        // Don't automatically create a new instance
                        return null;
                    }
                    instance.Initialize();
                }
                return instance;
            }
        }

        #endregion

        #region Unity Messages

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                var instances = FindObjectsOfType<MetaMaskUnityTransportBroadcaster>();
                if (instances.Length > 1)
                {
                    Debug.LogError("There are more than 1 instances of " + nameof(MetaMaskUnityTransportBroadcaster) + " inside the scene, there should be only one.");
                    Destroy(gameObject);
                    return;
                }
            }
            Initialize();
        }

        #endregion

        #region Protected Methods

        protected void Initialize()
        {
            for (int i = 0; i < this.listeners.Length; i++)
            {
                var listener = this.listeners[i];
                if (listener != null)
                {
                    var metamaskListener = listener.GetComponent<IMetaMaskUnityTransportListener>();
                    if (metamaskListener != null)
                    {
                        this.allListeners.Add(metamaskListener);
                    }
                }
            }
        }

        protected static MetaMaskUnityTransportBroadcaster CreateNewInstance()
        {
            var go = new GameObject(nameof(MetaMaskUnityTransportBroadcaster));
            DontDestroyOnLoad(go);
            return go.AddComponent<MetaMaskUnityTransportBroadcaster>();
        }

        #endregion

        #region MetaMask Events

        public void OnMetaMaskConnectRequest(string universalLink, string deepLink)
        {
            for (int i = 0; i < this.allListeners.Count; i++)
            {
                var listener = this.allListeners[i];
                if (listener != null)
                {
                    UnityThread.executeInUpdate(() => { listener.OnMetaMaskConnectRequest(universalLink, deepLink); });
                }
            }
        }
        
        public void OnMetaMaskOTPCode(int code)
        {
            for (int i = 0; i < this.allListeners.Count; i++)
            {
                var listener = this.allListeners[i];
                if (listener != null)
                {
                    UnityThread.executeInUpdate(() => { listener.OnMetaMaskOTP(code); });
                }
            }
        }

        public void OnMetaMaskFailure(Exception error)
        {
            for (int i = 0; i < this.allListeners.Count; i++)
            {
                var listener = this.allListeners[i];
                if (listener != null)
                {
                    UnityThread.executeInUpdate(() => { listener.OnMetaMaskFailure(error); });
                }
            }
        }

        public void OnMetaMaskRequest(string id, MetaMaskEthereumRequest request)
        {
            for (int i = 0; i < this.allListeners.Count; i++)
            {
                var listener = this.allListeners[i];
                if (listener != null)
                {
                    UnityThread.executeInUpdate(() => { listener.OnMetaMaskRequest(id, request); });
                }
            }
        }

        public void OnMetaMaskSuccess()
        {
            for (int i = 0; i < this.allListeners.Count; i++)
            {
                var listener = this.allListeners[i];
                if (listener != null)
                {
                    UnityThread.executeInUpdate(() => { listener.OnMetaMaskSuccess(); });
                }
            }
        }

        #endregion
    }

}