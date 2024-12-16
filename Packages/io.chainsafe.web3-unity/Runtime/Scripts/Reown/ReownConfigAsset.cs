using System;
using System.Collections.Generic;
using ChainSafe.Gaming.Reown.Connection;
using ChainSafe.Gaming.Reown.Dialog;
using ChainSafe.Gaming.Reown.Wallets;
using Reown.Core;
using Reown.Core.Network;
using Reown.Core.Network.Interfaces;
using UnityEngine;

namespace ChainSafe.Gaming.Reown
{
    [CreateAssetMenu(menuName = "ChainSafe/Reown/Reown Config", fileName = "ReownConfig", order = 0)]
    public class ReownConfigAsset : ScriptableObject, IReownConfig
    {
        public string SignMessageRpcMethodName => "personal_sign";
        public string SignTypedMessageRpcMethodName => "eth_signTypedData";

        [SerializeField] private List<string> includeWalletIds;
        [SerializeField] private List<string> excludeWalletIds;

        [field: SerializeField] public bool AutoRenewSession { get; set; } = true;
        [field: SerializeField] public string ProjectName { get; set; }
        [field: SerializeField] public string ProjectId { get; set; }
        [field: SerializeField] public string BaseContext { get; set; } = "unity-game";
        [field: SerializeField] public Metadata Metadata { get; set; }

        [field: SerializeField] public string StoragePath { get; set; } = "wallet-connect/";
        [field: SerializeField] public string OverrideRegistryUri { get; set; }
        [field: SerializeField] public ReownLogLevel LogLevel { get; set; } = ReownLogLevel.ErrorOnly;
        [SerializeField] private ConnectionHandlerProviderAsset connectionHandlerProvider;
        [field: SerializeField] public WalletLocationOption WalletLocationOption { get; set; }

        public bool RememberSession { get; set; }
        public bool ForceNewSession { get; set; }
        public EventHandler<Exception> OnRelayErrored { get; set; }
        
        public IRelayUrlBuilder RelayUrlBuilder => null; // todo;

        public IList<string> IncludeWalletIds => includeWalletIds;
        public IList<string> ExcludeWalletIds => excludeWalletIds;
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