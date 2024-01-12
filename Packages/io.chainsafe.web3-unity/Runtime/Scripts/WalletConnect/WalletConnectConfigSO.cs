using System.Collections.Generic;
using System.Threading.Tasks;
using ChainSafe.Gaming.WalletConnect.Dialog;
using JetBrains.Annotations;
using UnityEngine;
using WalletConnectSharp.Core;
using WalletConnectSharp.Network.Interfaces;

namespace ChainSafe.Gaming.WalletConnect
{
    [CreateAssetMenu(menuName = "ChainSafe/WalletConnect/WalletConnect Config", fileName = "WalletConnectConfig", order = 0)]
    public class WalletConnectConfigSO : ScriptableObject, IWalletConnectConfigNew
    {
        [field: SerializeField] public bool AutoRenewSession { get; set; } = true;
        [field: SerializeField] public string ProjectName { get; set; }
        [field: SerializeField] public string ProjectId { get; set; }
        [field: SerializeField] public string BaseContext { get; set; }
        [field: SerializeField] public Metadata Metadata { get; set; }
        [field: SerializeField] public string StoragePath { get; set; } = "wallet-connect/";
        [field: SerializeField] public string OverrideRegistryUri { get; set; }
        [SerializeField] private List<string> enabledWallets;
        [SerializeField] private List<string> disabledWallets;
        [SerializeField] private ConnectionDialogProviderSO connectionDialogProvider;
        [field: SerializeField] public WalletLocationOptions WalletLocationOptions { get; set; }
        
        public bool RememberSession { get; set; }
        public IList<string> EnabledWallets => enabledWallets;
        public IList<string> DisabledWallets => disabledWallets;
        public IConnectionDialogProvider ConnectionDialogProvider => connectionDialogProvider;

        private IConnectionBuilder connectionBuilder;

        public IConnectionBuilder ConnectionBuilder
        {
            get
            {
#if !UNITY_2022_1_OR_NEWER

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
#else
            return null;
#endif
            }
        }
    }
}