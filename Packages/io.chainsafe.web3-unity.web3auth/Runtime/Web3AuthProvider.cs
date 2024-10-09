using System.Threading.Tasks;
using ChainSafe.Gaming.Evm;
using ChainSafe.Gaming.InProcessSigner;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Environment;
using ChainSafe.Gaming.Web3.Evm.Wallet;
using ChainSafe.GamingSdk.Web3Auth;
using Nethereum.RPC.Accounts;
using Nethereum.Web3.Accounts;
using UnityEngine;

/// <summary>
/// Web3Auth provider allowing users to connect a Web3Auth wallet.
/// </summary>
public class Web3AuthProvider : WalletProvider, IAccountProvider
{
    private readonly Web3AuthWalletConfig _config;

    private Web3Auth _coreInstance;
    private TaskCompletionSource<Web3AuthResponse> _connectTcs;
    private TaskCompletionSource<object> _disconnectTcs;

    public Web3AuthProvider(Web3AuthWalletConfig config, Web3Environment environment, IChainConfig chainConfig) : base(environment, chainConfig)
    {
        _config = config;
    }

    public IAccount Account { get; private set; }

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

        if (_connectTcs != null && !_connectTcs.Task.IsCompleted)
        {
            Cancel();
        }

        _connectTcs = new TaskCompletionSource<Web3AuthResponse>();

        _coreInstance.onLogin += OnLogin;

        _coreInstance.Initialize();

        _coreInstance.setOptions(_config.Web3AuthOptions, _config.RememberMe);

        var providerTask = _config.ProviderTask;

        if (!_config.AutoLogin && providerTask != null
            //On webGL providerTask is always completed, so we don't have to go through another login flow.
#if UNITY_WEBGL && !UNITY_EDITOR
                               && !providerTask.IsCompleted
#endif
            )
        {
            var provider = await providerTask;

            _coreInstance.login(new LoginParams
            {
                loginProvider = provider,
            });
        }

        await using (_config.CancellationToken.Register(Cancel))
        {
            var response = await _connectTcs.Task;

            Account = new Account(response.privKey);

            Account.TransactionManager.Client = this;

            return Account.Address;
        }
    }

    private void Cancel()
    {
        _coreInstance.onLogin -= OnLogin;

        _connectTcs.SetCanceled();
    }

    private void OnLogin(Web3AuthResponse response)
    {
        _coreInstance.onLogin -= OnLogin;

        if (string.IsNullOrEmpty(response.error))
        {
            _connectTcs.SetResult(response);
        }

        else
        {
            _connectTcs.SetException(new Web3Exception(response.error));
        }
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
