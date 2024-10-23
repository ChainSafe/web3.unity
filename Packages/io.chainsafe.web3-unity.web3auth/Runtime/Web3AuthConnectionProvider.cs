using System;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using AOT;
using ChainSafe.Gaming;
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
public class Web3AuthConnectionProvider : ConnectionProvider, ILogoutHandler, IWeb3InitializedHandler
{
    [field: SerializeField, DefaultAssetValue("Packages/io.chainsafe.web3-unity.web3auth/Runtime/Prefabs/Web3AuthRow.prefab")]
    public override Button ConnectButtonRow { get; protected set; }

    [SerializeField] private string clientId;
    [SerializeField] private string redirectUri;
    [SerializeField] private Network network;

    [Space]

    [SerializeField, DefaultAssetValue("Packages/io.chainsafe.web3-unity.web3auth/Runtime/Prefabs/Web3Auth.prefab")]
    private GameObject modalPrefab;

    [Space]

    [SerializeField] private bool enableWalletGui;

    [SerializeField, DefaultAssetValue("Packages/io.chainsafe.web3-unity.web3auth/Runtime/WalletGUI/Prefabs/Web3AuthWalletGUI.prefab")]
    private Web3AuthWalletGUI web3AuthWalletGUIPrefab;

    [SerializeField] private Web3AuthWalletGUI.Web3AuthWalletConfig walletGuiConfig;

    private Web3AuthModal _modal;

    private Web3AuthWalletGUI _web3AuthWalletGui;

    [NonSerialized] private bool _rememberMe;

    public override bool IsAvailable => true;

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
            chainConfig.Rpc, chainConfig.Network, "", chainConfig.Symbol, "", network.ToString().ToLower(), Initialized, InitializeError);

        await _initializeTcs.Task;
    }
#endif

    protected override void ConfigureServices(IWeb3ServiceCollection services)
    {
        // Don't display modal if it's an auto login.
        if (!_rememberMe)
        {
            DisplayModal();
        }

        var web3AuthConfig = new Web3AuthWalletConfig
        {
            Web3AuthOptions = new()
            {
                clientId = clientId,
                redirectUrl = new Uri(redirectUri),
                network = network,
                whiteLabel = new()
                {
                    mode = Web3Auth.ThemeModes.dark,
                    defaultLanguage = Web3Auth.Language.en,
                    appName = "ChainSafe Gaming SDK",
                }
            },
            RememberMe = _rememberMe || RememberSession,

            AutoLogin = _rememberMe,
            UseWalletGui = enableWalletGui
        };

        web3AuthConfig.CancellationToken = _rememberMe ? default : _modal.CancellationToken;

        web3AuthConfig.ProviderTask = _rememberMe ? default : _modal.SelectProvider();

#if UNITY_WEBGL && !UNITY_EDITOR
            web3AuthConfig.CancellationToken.Register(delegate
            {
                if (_connectionTcs != null && !_connectionTcs.Task.IsCompleted)
                {
                    _connectionTcs.SetCanceled();
                }
            });
            
            web3AuthConfig.SessionTask = Connect();
#endif

        services.UseWeb3AuthWallet(web3AuthConfig);

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
        if (_modal != null)
        {
            _modal.gameObject.SetActive(true);
        }

        else
        {
            var obj = Instantiate(modalPrefab);

            _modal = obj.GetComponentInChildren<Web3AuthModal>();
        }
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

        if (enableWalletGui && _web3AuthWalletGui != null)
        {
            Destroy(_web3AuthWalletGui.gameObject);
        }

        return Task.CompletedTask;
    }

    public Task OnWeb3Initialized(Web3 web3)
    {
        if (enableWalletGui)
        {
            _web3AuthWalletGui = Instantiate(web3AuthWalletGUIPrefab);
            _web3AuthWalletGui.Initialize(walletGuiConfig);
        }

        if (_modal != null)
        {
            _modal?.Close();
        }

        return Task.CompletedTask;
    }
}
