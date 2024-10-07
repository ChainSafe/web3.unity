using System.Collections.Generic;
using ChainSafe.Gaming.WalletConnect.Connection;
using ChainSafe.Gaming.WalletConnect.Dialog;
using UnityEngine;
using WalletConnectSharp.Core;
using WalletConnectSharp.Network.Interfaces;

namespace ChainSafe.Gaming.WalletConnect
{
    [CreateAssetMenu(menuName = "ChainSafe/WalletConnect/WalletConnect Config", fileName = "WalletConnectConfig", order = 0)]
    public class WalletConnectConfigSO : ScriptableObject, IWalletConnectConfig
    {
        public string SignMessageRpcMethodName => "personal_sign";
        public string SignTypedMessageRpcMethodName => "eth_signTypedData";

        [field: SerializeField] public bool AutoRenewSession { get; set; } = true;
        [field: SerializeField] public string ProjectName { get; set; }
        [field: SerializeField] public string ProjectId { get; set; }
        [field: SerializeField] public string BaseContext { get; set; } = "unity-game";
        [field: SerializeField] public Metadata Metadata { get; set; }
        [field: SerializeField] public string StoragePath { get; set; } = "wallet-connect/";
        [field: SerializeField] public string OverrideRegistryUri { get; set; }
        [field: SerializeField] public WalletConnectLogLevel LogLevel { get; set; } = WalletConnectLogLevel.ErrorOnly;
        [SerializeField] private List<string> enabledWallets;
        [SerializeField] private List<string> disabledWallets;
        [SerializeField] private ConnectionHandlerProviderSO connectionHandlerProvider;
        [field: SerializeField] public WalletLocationOption WalletLocationOption { get; set; }

        public bool RememberSession { get; set; }
        public bool ForceNewSession { get; set; }
        public IList<string> EnabledWallets => enabledWallets;
        public IList<string> DisabledWallets => disabledWallets;
        public IConnectionHandlerProvider ConnectionHandlerProvider => connectionHandlerProvider;

        private IConnectionBuilder connectionBuilder;

        public IConnectionBuilder ConnectionBuilder
        {
            get
            {

                if (connectionBuilder != null)
                {
                    return connectionBuilder;
                }

                connectionBuilder = FindObjectOfType<NativeWebSocketConnectionBuilder>();

                if (connectionBuilder != null)
                {
                    return connectionBuilder;
                }

                // Initialize custom web socket if it's not already.
                var webSocketBuilderObj =
                    new GameObject(nameof(NativeWebSocketConnectionBuilder), typeof(NativeWebSocketConnectionBuilder))
                    {
                        hideFlags = HideFlags.HideAndDontSave
                    };
                DontDestroyOnLoad(webSocketBuilderObj);

                connectionBuilder = webSocketBuilderObj.GetComponent<NativeWebSocketConnectionBuilder>();

                return connectionBuilder;
            }
        }
    }
}