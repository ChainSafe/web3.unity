using System;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using AOT;
using ChainSafe.Gaming;
using ChainSafe.Gaming.GUI;
using ChainSafe.Gaming.UnityPackage;
using ChainSafe.Gaming.UnityPackage.Connection;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Core.Logout;
using ChainSafe.GamingSdk.Web3Auth;
using Microsoft.Extensions.DependencyInjection;
using Nethereum.Hex.HexTypes;
using UnityEngine;
using UnityEngine.UI;
using Network = Web3Auth.Network;

/// <summary>
/// ConnectionProvider for connecting wallet via Web3Auth.
/// </summary>
[CreateAssetMenu(menuName = "ChainSafe/Connection Provider/Web3Auth", fileName = nameof(Web3AuthConnectionProvider))]
public class Web3AuthConnectionProvider : ConnectionProvider, ILogoutHandler, IWeb3InitializedHandler, IWeb3AuthConfig
{
    [field: SerializeField, DefaultAssetValue("Packages/io.chainsafe.web3-unity.web3auth/Runtime/Sprites/web3auth.png")]
    public override Sprite ButtonIcon { get; protected set; }

    [field: SerializeField] public override string ButtonText { get; protected set; } = "Web3Auth";

    [field: Space] [field: SerializeField] public string AppName { get; private set; } = "ChainSafe Gaming SDK";
    [field: SerializeField] public string ClientId { get; private set; }
    
    [field: SerializeField] public string RedirectUri { get; private set; }
    
    [field: SerializeField] public Network Network { get; private set; }
    [field: SerializeField] public Web3Auth.ThemeModes Theme { get; private set; } = Web3Auth.ThemeModes.dark;
    [field: SerializeField] public Web3Auth.Language Language { get; private set; } = Web3Auth.Language.en;

    [Space]

    [SerializeField]
    private GuiScreenFactory modalScreenFactory;
    
    [Space]

    [SerializeField] private bool enableWalletGui;

    [SerializeField]
    private GuiScreenFactory embeddedWalletScreenFactory;

    [SerializeField] private Web3AuthWalletGUI.Web3AuthWalletConfig walletGuiConfig;

    private Web3AuthModal _modal;

    private EmbeddedWalletScreen _embeddedWallet;

    [NonSerialized] private bool _rememberMe;

    public override bool IsAvailable => true;

    public Task<string> SessionTask { get; private set; }
    
    public Task<Provider> ProviderTask => _rememberMe ? default : _modal.SelectProvider();
    
    public CancellationToken CancellationToken => _rememberMe ? default : _modal.CancellationToken;

    public bool RememberMe => _rememberMe || RememberSession;

    public bool AutoLogin => _rememberMe;
    
#if UNITY_WEBGL && !UNITY_EDITOR

    private TaskCompletionSource<string> _initializeTcs;
    
    private TaskCompletionSource<string> _connectionTcs;
    
    [DllImport("__Internal")]
    private static extern void InitWeb3Auth(string clientId, string chainId, string rpcTarget, string displayName, string blockExplorerUrl, string ticker, string tickerName, string network, Action callback, Action<string> fallback);

    [DllImport("__Internal")]
    private static extern void Web3AuthLogin(string provider, bool rememberMe, Action<string> callback, Action<string> fallback);
    
    private static Web3AuthConnectionProvider _instance;
    
    private void Awake()
    {
        _instance = this;
    }

    public override async Task Initialize(bool rememberSession)
    {
        await base.Initialize(rememberSession);
        
        _initializeTcs = new TaskCompletionSource<string>();
        
        var projectConfig = ProjectConfigUtilities.Load();

        var chainConfig = projectConfig.ChainConfigs.FirstOrDefault();

        if (chainConfig is null)
        {
            Debug.LogError($"Couldn't initialize {nameof(Web3AuthConnectionProvider)}. No Chain Config were found in Project Config.");
            return;
        }
        
        //1155 is a decimal number, we need to convert it to an integer
        InitWeb3Auth(clientId, new HexBigInteger(BigInteger.Parse(chainConfig.ChainId)).HexValue, 
            chainConfig.Rpc, chainConfig.Network, "", chainConfig.NativeCurrency.Symbol, "", network.ToString().ToLower(), Initialized, InitializeError);

        await _initializeTcs.Task;
    }
#endif
    
#if UNITY_EDITOR
    private void OnValidate()
    {
        if (!modalScreenFactory.LandscapePrefab && !modalScreenFactory.PortraitPrefab)
        {
            modalScreenFactory.LandscapePrefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GuiScreen>(UnityEditor.AssetDatabase.GUIDToAssetPath("9e5f859444d8b4a448e79b28a6033fd7"));
            modalScreenFactory.PortraitPrefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GuiScreen>(UnityEditor.AssetDatabase.GUIDToAssetPath("5ed2d6739dc24144cb021a0cb4bd8178"));
        }
    }
#endif

    protected override void ConfigureServices(IWeb3ServiceCollection services)
    {
        // Don't display modal if it's an auto login.
        if (!_rememberMe)
        {
            DisplayModal();
        }

#if UNITY_WEBGL && !UNITY_EDITOR
            CancellationToken.Register(delegate
            {
                if (_connectionTcs != null && !_connectionTcs.Task.IsCompleted)
                {
                    _connectionTcs.SetCanceled();
                }
            });
            
            SessionTask = Connect();
#endif

        services.UseWeb3AuthWallet(this);

        services.AddSingleton<ILogoutHandler, IWeb3InitializedHandler, Web3AuthConnectionProvider>(_ => this);
    }

    public override Task<bool> SavedSessionAvailable()
    {
        if (!string.IsNullOrEmpty(KeyStoreManagerUtils.getPreferencesData(KeyStoreManagerUtils.SESSION_ID)))
        {
            _rememberMe = true;
        }

        return Task.FromResult(_rememberMe);
    }

    public override void HandleException(Exception exception)
    {
        _rememberMe = false;

        if (_modal != null)
        {
            _modal.Close();
        }

        base.HandleException(exception);
    }

    private void DisplayModal()
    {
        _modal = modalScreenFactory.GetSingle<Web3AuthModal>();
        _modal.gameObject.SetActive(true);
    }

 #if UNITY_WEBGL && !UNITY_EDITOR
    
    [MonoPInvokeCallback(typeof(Action))]
    private static void Initialized()
    {
        Debug.Log("Web3Auth Initialized Successfully.");

        _instance._initializeTcs.SetResult(string.Empty);
    }
    
    [MonoPInvokeCallback(typeof(Action<string>))]
    private static void InitializeError(string message)
    {
        _instance._initializeTcs.SetException(new Web3Exception(message));
    }

    private async Task<string> Connect()
    {
        if (_rememberMe)
        {
            return KeyStoreManagerUtils.getPreferencesData(KeyStoreManagerUtils.SESSION_ID);
        }
        
        if (_connectionTcs != null && !_connectionTcs.Task.IsCompleted)
        {
            _connectionTcs.SetCanceled();
        }
        
        _connectionTcs = new TaskCompletionSource<string>();
        
        var provider = await _modal.SelectProvider();
        
        Web3AuthLogin(provider.ToString().ToLower(), _rememberMe || RememberSession, Connected, ConnectError);

        return await _connectionTcs.Task;
    }
    
    [MonoPInvokeCallback(typeof(Action<string>))]
    private static void Connected(string sessionId)
    {
        _instance._connectionTcs.SetResult(sessionId);
    }
    
    [MonoPInvokeCallback(typeof(Action<string>))]
    private static void ConnectError(string message)
    {
        _instance._connectionTcs.SetException(new Web3Exception(message));
    }
#endif

    public Task OnLogout()
    {
        _rememberMe = false;

        if (enableWalletGui && _embeddedWallet != null)
        {
            Destroy(_embeddedWallet.gameObject);
        }

        return Task.CompletedTask;
    }

    public Task OnWeb3Initialized(Web3 web3)
    {
        if (enableWalletGui)
        {
            _embeddedWallet = embeddedWalletScreenFactory.GetSingle<EmbeddedWalletScreen>();
            _embeddedWallet.Initialize(web3);
            _embeddedWallet.gameObject.SetActive(true);
        }

        if (_modal != null)
        {
            _modal.Close();
        }

        return Task.CompletedTask;
    }

    public bool AutoApproveTransactions => !enableWalletGui;
}
