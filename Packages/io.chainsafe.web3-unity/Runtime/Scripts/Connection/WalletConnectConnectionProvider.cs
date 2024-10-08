using System.Collections.Generic;
using System.Threading.Tasks;
using ChainSafe.Gaming.WalletConnect;
using ChainSafe.Gaming.WalletConnect.Connection;
using ChainSafe.Gaming.WalletConnect.Dialog;
using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Evm.Wallet;
using UnityEngine;
using WalletConnectSharp.Core;
using WalletConnectSharp.Network.Interfaces;
using WConnectionHandler = ChainSafe.Gaming.WalletConnect.Connection.IConnectionHandler;
using UnityEngine.UI;

namespace ChainSafe.Gaming.UnityPackage.Connection
{
    /// <summary>
    /// WalletConnect connection provider used for connecting to a wallet using WalletConnect.
    /// </summary>
    [CreateAssetMenu(menuName = "ChainSafe/Connection Provider/Wallet Connect", fileName = nameof(WalletConnectConnectionProvider))]
    public class WalletConnectConnectionProvider : ConnectionProvider, IWalletConnectConfig, IConnectionHandlerProvider
    {
        [field: SerializeField] public string ProjectId { get; private set; }

        [field: SerializeField] public string ProjectName { get; private set; }

        [field: SerializeField] public bool AutoRenewSession { get; private set; } = true;

        [field: SerializeField] public string BaseContext { get; private set; } = "unity-game";

        [field: SerializeField] public Metadata Metadata { get; private set; }

        [field: SerializeField] public string StoragePath { get; private set; } = "wallet-connect/";

        [field: SerializeField] public string OverrideRegistryUri { get; private set; }

        [DefaultAssetValue("Packages/io.chainsafe.web3-unity/Runtime/Prefabs/Wallet Connect/WalletConnect Dialog.prefab")]
        [SerializeField] private ConnectionHandlerBehaviour handlerPrefab;

        [SerializeField] private List<string> enabledWallets;

        [SerializeField] private List<string> disabledWallets;

        [field: SerializeField] public WalletConnectLogLevel LogLevel { get; private set; } = WalletConnectLogLevel.ErrorOnly;

        [field: SerializeField]
        public WalletLocationOption WalletLocationOption { get; private set; } = WalletLocationOption.LocalAndRemote;

        [field: SerializeField, DefaultAssetValue("Packages/io.chainsafe.web3-unity/Runtime/Prefabs/WalletConnectRow.prefab")]
        public override Button ConnectButtonRow { get; protected set; }

        private bool _storedSessionAvailable;

        private ConnectionHandlerBehaviour _loadedHandler;

        public string SignMessageRpcMethodName => "personal_sign";
        public string SignTypedMessageRpcMethodName => "eth_signTypedData";

        public IList<string> EnabledWallets => enabledWallets;
        public IList<string> DisabledWallets => disabledWallets;

        bool IWalletConnectConfig.RememberSession => RememberSession || _storedSessionAvailable;

        public IConnectionHandlerProvider ConnectionHandlerProvider => this;

        public bool ForceNewSession { get; set; }

        public override bool IsAvailable => true;

        private IConnectionBuilder _connectionBuilder;

        public IConnectionBuilder ConnectionBuilder
        {
            get
            {

                if (_connectionBuilder != null)
                {
                    return _connectionBuilder;
                }

                _connectionBuilder = FindObjectOfType<NativeWebSocketConnectionBuilder>();

                if (_connectionBuilder != null)
                {
                    return _connectionBuilder;
                }

                // Initialize custom web socket if it's not already.
                var webSocketBuilderObj =
                    new GameObject(nameof(NativeWebSocketConnectionBuilder), typeof(NativeWebSocketConnectionBuilder))
                    {
                        hideFlags = HideFlags.HideAndDontSave
                    };
                DontDestroyOnLoad(webSocketBuilderObj);

                _connectionBuilder = webSocketBuilderObj.GetComponent<NativeWebSocketConnectionBuilder>();

                return _connectionBuilder;
            }
        }

        protected override void ConfigureServices(IWeb3ServiceCollection services)
        {
            services.UseWalletConnect(this)
                .UseWalletSigner().UseWalletTransactionExecutor();
        }

        public override async Task<bool> SavedSessionAvailable()
        {
            await using (var lightWeb3 = await WalletConnectWeb3.BuildLightweightWeb3(this))
            {
                _storedSessionAvailable = lightWeb3.WalletConnect().ConnectionHelper().StoredSessionAvailable;
            }

            return _storedSessionAvailable;
        }

        public Task<WConnectionHandler> ProvideHandler()
        {
            if (_loadedHandler != null)
            {
                return Task.FromResult((WConnectionHandler)_loadedHandler);
            }

            _loadedHandler = Instantiate(handlerPrefab);
            return Task.FromResult((WConnectionHandler)_loadedHandler);
        }
    }
}
