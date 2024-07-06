using System.Threading.Tasks;
using ChainSafe.Gaming.Evm;
using ChainSafe.Gaming.InProcessSigner;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Environment;
using ChainSafe.Gaming.Web3.Evm.Wallet;
using ChainSafe.GamingSdk.Web3Auth;
using Nethereum.Web3.Accounts;
using UnityEngine;

/// <summary>
/// Web3Auth provider allowing users to connect a Web3Auth wallet.
/// </summary>
public class Web3AuthProvider : WalletProvider
{
    private readonly Web3AuthWalletConfig _config;
    private readonly AccountProvider _accountProvider;
    
    private Web3Auth _coreInstance;
    private TaskCompletionSource<Web3AuthResponse> _connectTcs;
    private TaskCompletionSource<object> _disconnectTcs;

    public Web3AuthProvider(Web3AuthWalletConfig config, AccountProvider accountProvider, Web3Environment environment, IChainConfig chainConfig, ChainRegistryProvider chainRegistryProvider) : base(environment, chainRegistryProvider, chainConfig)
    {
        _config = config;
        _accountProvider = accountProvider;
    }

    /// <summary>
    /// Connects Web3Auth wallet.
    /// </summary>
    /// <returns>Connected Account.</returns>
    public override async Task<string> Connect()
    {
        _coreInstance = Object.FindObjectOfType<Web3Auth>();
        
        if (_coreInstance == null)
        {
            var gameObject = new GameObject("Web3Auth", typeof(Web3Auth));
        
            Object.DontDestroyOnLoad(gameObject);

            _coreInstance = gameObject.GetComponent<Web3Auth>();
        }
        
        _coreInstance.setOptions(_config.Web3AuthOptions, _config.RememberMe);

        if (_connectTcs != null && !_connectTcs.Task.IsCompleted)
        {
            _connectTcs.SetCanceled();
        }
        
        _connectTcs = new TaskCompletionSource<Web3AuthResponse>();
        
        _coreInstance.onLogin += OnLogin;

        if (_config.LoginParams != null)
        {
            _coreInstance.login(_config.LoginParams);
        }

        var response = await _connectTcs.Task;
        
        var account = new Account(response.privKey);

        account.TransactionManager.Client = this;
        
        _accountProvider.Initialize(account);
        
        return account.Address;
    }

    private void OnLogin(Web3AuthResponse response)
    {
        _connectTcs.SetResult(response);
    }
    
    /// <summary>
    /// Disconnect Web3Auth wallet.
    /// </summary>
    public override async Task Disconnect()
    {
        if (_disconnectTcs != null && !_disconnectTcs.Task.IsCompleted)
        {
            _disconnectTcs.SetCanceled();
        }
        
        _disconnectTcs = new TaskCompletionSource<object>();
        
        _coreInstance.onLogout += OnLogout;
        
        _coreInstance.logout();

        await _disconnectTcs.Task;
        
        Object.Destroy(_coreInstance.gameObject);
        
        void OnLogout()
        {
            _disconnectTcs.SetResult(null);
        }
    }

    /// <summary>
    /// Make RPC requests to the Web3Auth wallet.
    /// </summary>
    /// <param name="method">RPC request method.</param>
    /// <param name="parameters">RPC request parameters.</param>
    /// <typeparam name="T">Type of response.</typeparam>
    /// <returns>RPC request response.</returns>
    public override Task<T> Request<T>(string method, params object[] parameters)
    {
        return Perform<T>(method, parameters);
    }
}
