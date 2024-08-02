using System;
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
using Network = Web3Auth.Network;

/// <summary>
/// ConnectionProvider for connecting wallet via Web3Auth.
/// </summary>
[CreateAssetMenu(menuName = "ChainSafe/Connection Provider/Web3Auth", fileName = nameof(Web3AuthConnectionProvider))]
public class Web3AuthConnectionProvider : RestorableConnectionProvider, ILogoutHandler
{
    [SerializeField] private string clientId;
    [SerializeField] private string redirectUri;
    [SerializeField] private Network network;
    
    [Space]
    
    [SerializeField] public GameObject modalPrefab;
    
    private Web3AuthModal _modal;

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

    public override async Task Initialize()
    {
        _initializeTcs = new TaskCompletionSource<string>();
        
        var projectConfig = ProjectConfigUtilities.Load();
        
        //1155 is a decimal number, we need to convert it to an integer
        InitWeb3Auth(clientId, new HexBigInteger(BigInteger.Parse(projectConfig.ChainId)).HexValue, 
            projectConfig.Rpc, projectConfig.Network, "", projectConfig.Symbol, "", network.ToString().ToLower(), Initialized, InitializeError);

        await _initializeTcs.Task;
        
        Debug.Log("Web3Auth Initialized Successfully.");
    }
#else
    public override Task Initialize()
    {
        return Task.CompletedTask;
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
            
            AutoLogin = _rememberMe
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
        
        services.AddSingleton<ILogoutHandler>(this);
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
        _modal.Close();
        
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
        
        return Task.CompletedTask;
    }
}
