using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChainSafe.Gaming.GUI;
using ChainSafe.Gaming.Reown;
using ChainSafe.Gaming.Reown.AppKit;
using ChainSafe.Gaming.Reown.Connection;
using ChainSafe.Gaming.Reown.Dialog;
using ChainSafe.Gaming.Reown.Wallets;
using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Evm.Wallet;
using Newtonsoft.Json;
using Reown.Core;
using Reown.Core.Network;
using Reown.Core.Network.Interfaces;
using UnityEngine;
using ReownConnectionHandler = ChainSafe.Gaming.Reown.Connection.IConnectionHandler;

namespace ChainSafe.Gaming.UnityPackage.Connection
{
    /// <summary>
    /// Reown connection provider used for connecting to a wallet using Reown.
    /// </summary>
    [CreateAssetMenu(menuName = "ChainSafe/Connection Provider/Reown", fileName = nameof(ReownConnectionProvider))]
    public class ReownConnectionProvider : ConnectionProvider, IReownConfig, IConnectionHandlerProvider
    {
        [SerializeField] private GuiScreenFactory connectionScreenPrefabs;
        [SerializeField] private List<string> includeWalletIds;
        [SerializeField] private List<string> excludeWalletIds;

        [field: SerializeField] public string ProjectId { get; private set; }

        [field: SerializeField] public string ProjectName { get; private set; }

        [field: SerializeField] public bool AutoRenewSession { get; private set; } = true;

        [field: SerializeField] public string BaseContext { get; private set; } = "unity-game";

        [field: SerializeField] public Metadata Metadata { get; private set; } = new Metadata(){ Description = "Web3.Unity SDK is an open source SDK that connects unity games to the blockchain", Name = "Web3.Unity SDK", Url = "https://chainsafe.io"};

        [field: SerializeField] public string OverrideRegistryUri { get; private set; }

        [field: SerializeField] public ReownLogLevel LogLevel { get; private set; } = ReownLogLevel.ErrorOnly;

        [field: SerializeField]
        public WalletLocationOption WalletLocationOption { get; private set; } = WalletLocationOption.LocalAndRemote;
        
        [field:SerializeField]
        [Tooltip("")]
        public ViemNameChainId[] ChainIdAndViemNameArray { get; private set; }

        [field: SerializeField]
        public override Sprite ButtonIcon { get; protected set; }

        [field: SerializeField] public override string ButtonText { get; protected set; } = "Reown";

        private bool _storedSessionAvailable;
        private ConnectionHandlerBehaviour _loadedHandler;
        private IConnectionBuilder _connectionBuilder;

        public bool ForceNewSession { get; set; }
        public EventHandler<Exception> OnRelayErrored { get; set; }

        public Dictionary<string, string> ChainIdViemNameMap { get; private set; } = new();

        bool IReownConfig.RememberSession => RememberSession || _storedSessionAvailable;
        public IList<string> IncludeWalletIds => includeWalletIds;
        public IList<string> ExcludeWalletIds => excludeWalletIds;
        public IConnectionHandlerProvider ConnectionHandlerProvider => this;
        public IRelayUrlBuilder RelayUrlBuilder => null;
        public string SignMessageRpcMethodName => "personal_sign";
        public string SignTypedMessageRpcMethodName => "eth_signTypedData";
        public override bool IsAvailable => true;
        
        //We need to serialize this bad boy.
        [SerializeField, HideInInspector] private ViemNameChainId[] allChainIdsAndViemNames;

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
    
#if UNITY_EDITOR
        private void OnValidate()
        {
            if (!connectionScreenPrefabs.LandscapePrefab && !connectionScreenPrefabs.PortraitPrefab)
            {
                connectionScreenPrefabs.LandscapePrefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GuiScreen>(UnityEditor.AssetDatabase.GUIDToAssetPath("344d6e9400e973843b2b68a8f4786e0b"));
                connectionScreenPrefabs.PortraitPrefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GuiScreen>(UnityEditor.AssetDatabase.GUIDToAssetPath("f844207643fe35744b72bf387a704960"));
            }

           
#if UNITY_WEBGL && UNITY_EDITOR
            if(allChainIdsAndViemNames == null || allChainIdsAndViemNames.Length == 0)
                allChainIdsAndViemNames = JsonConvert.DeserializeObject<ViemNameChainId[]>(Resources.Load<TextAsset>("ViemChain").text);
            
            if (ChainIdAndViemNameArray == null || ChainIdAndViemNameArray.Length == 0)
            {
                var projectConfig = ProjectConfigUtilities.Load();
                var dict = projectConfig.ChainConfigs.ToDictionary(x => x.ChainId, x => x);
                ChainIdAndViemNameArray = allChainIdsAndViemNames.Where(x => dict.ContainsKey(x.ChainId)).ToArray();
            }
#endif
        }
#endif

        
        protected override void ConfigureServices(IWeb3ServiceCollection services)
        {
            #if UNITY_WEBGL && !UNITY_EDITOR
            ChainIdViemNameMap = ChainIdViemNameMap.Count == 0 ? ChainIdAndViemNameArray.ToDictionary(x => x.ChainId, x => x.ViewName) : ChainIdViemNameMap;
            services.UseAppKit(this)
                .UseWalletSigner()
                .UseWalletTransactionExecutor();

            #else
            services.UseReown(this)
                .UseWalletSigner()
                .UseWalletTransactionExecutor();
             
            #endif
        }
       

        public override async Task<bool> SavedSessionAvailable()
        {
            await using (var lightWeb3 = await ReownWeb3.BuildLightweightWeb3(this))
            {
                _storedSessionAvailable = lightWeb3.Reown().ConnectionHelper().StoredSessionAvailable;
            }

            return _storedSessionAvailable;
        }

        public Task<ReownConnectionHandler> ProvideHandler()
        {
            if (_loadedHandler != null)
            {
                return Task.FromResult((ReownConnectionHandler)_loadedHandler);
            }

            _loadedHandler = connectionScreenPrefabs.Build<ConnectionHandlerBehaviour>();
            return Task.FromResult((ReownConnectionHandler)_loadedHandler);
        }
    }
    
    [Serializable]
    public class ViemNameChainId
    {
        [JsonProperty("id")]
        [field:SerializeField]public string ChainId { get; set; }
        [JsonProperty("name")]
        [field:SerializeField]public string ViewName { get; set; }
    }
}
