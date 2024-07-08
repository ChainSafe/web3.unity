using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Providers;
using ChainSafe.Gaming.Evm.Signers;
using ChainSafe.Gaming.Evm.Transactions;
using ChainSafe.Gaming.InProcessSigner;
using ChainSafe.Gaming.InProcessTransactionExecutor;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Analytics;
using ChainSafe.Gaming.Web3.Core;
using ChainSafe.Gaming.Web3.Core.Evm;
using Nethereum.JsonRpc.Client;
using Nethereum.Signer;
using UnityEngine;
using Object = UnityEngine.Object;
using TWeb3Auth = Web3Auth;

namespace ChainSafe.GamingSdk.Web3Auth
{
    /// <summary>
    /// A class that represents a Web3Auth wallet and provides functionality for signing transactions and interacting with a blockchain.
    /// </summary>
    public class Web3AuthWallet : ISigner, ITransactionExecutor, ILifecycleParticipant
    {
        private readonly Web3AuthWalletConfig config;
        private readonly IRpcProvider rpcProvider;
        private TWeb3Auth coreInstance;
        private InProcessSigner signer;
        private InProcessTransactionExecutor transactionExecutor;
        private IClient rpcClient;
        private readonly IAnalyticsClient analyticsClient;
        private readonly IWeb3AuthTransactionHandler transactionHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="Web3AuthWallet"/> class.
        /// </summary>
        /// <param name="config">The configuration for the Web3Auth wallet.</param>
        /// <param name="rpcProvider">The RPC provider for blockchain interaction.</param>
        /// <param name="rpcClient"></param>
        /// <param name="analyticsClient"></param>
        public Web3AuthWallet(Web3AuthWalletConfig config, IRpcProvider rpcProvider, IClient rpcClient, IWeb3AuthTransactionHandler transactionHandler, IAnalyticsClient analyticsClient)
        {
            this.config = config;
            this.rpcProvider = rpcProvider;
            this.rpcClient = rpcClient;
            this.analyticsClient = analyticsClient;

            this.transactionHandler = transactionHandler;
        }

        /// <summary>
        /// Gets the blockchain address associated with this wallet.
        /// </summary>
        public string PublicAddress => signer.PublicAddress;

        /// <summary>
        /// Gets key.
        /// </summary>
        public string Key => signer.GetKey().GetPrivateKey();

        private readonly Dictionary<TransactionRequested, TaskCompletionSource<TransactionResponse>> _transactionPool =
            new Dictionary<TransactionRequested, TaskCompletionSource<TransactionResponse>>();

        /// <summary>
        /// Asynchronously prepares the Web3Auth wallet for operation, triggered when initializing the module in the dependency injection work flow.
        /// </summary>
        /// <returns>A <see cref="ValueTask"/> representing the asynchronous operation.</returns>
        public async ValueTask WillStartAsync()
        {

            analyticsClient.CaptureEvent(new AnalyticsEvent()
            {
                EventName = $"Web3Auth Initialized",
                PackageName = "io.chainsafe.web3-unity.web3auth",
            });
            coreInstance = CreateCoreInstance();
            TaskCompletionSource<string> loginTcs = new();
            coreInstance.onLogin += Web3Auth_OnLogin;

            if (config.LoginParams != null) coreInstance.login(config.LoginParams);

            var privateKeyString = await loginTcs.Task;
            var privateKey = new EthECKey(privateKeyString);

            var signerConfig = new InProcessSignerConfig { PrivateKey = privateKey };

            signer = new InProcessSigner(signerConfig);


            transactionExecutor = new InProcessTransactionExecutor(signer, analyticsClient.ChainConfig, rpcProvider, rpcClient);

            transactionHandler.OnTransactionApproved += OnTransactionApproved;
            transactionHandler.OnTransactionDeclined += OnTransactionDeclined;

            void Web3Auth_OnLogin(Web3AuthResponse response)
            {
                coreInstance.onLogin -= Web3Auth_OnLogin;
                loginTcs.SetResult(response.privKey);
            }
        }

        /// <summary>
        /// Asynchronously cleans up the Web3Auth wallet and logs out.
        /// </summary>
        /// <returns>A <see cref="ValueTask"/> representing the asynchronous operation.</returns>
        public async ValueTask WillStopAsync()
        {
            TaskCompletionSource<object> logoutTcs = new();
            coreInstance.onLogout += Web3Auth_OnLogout;
            coreInstance.logout();

            await logoutTcs.Task;

            coreInstance.onLogout -= Web3Auth_OnLogout;

            Object.Destroy(coreInstance.gameObject);

            transactionHandler.OnTransactionApproved += OnTransactionApproved;
            transactionHandler.OnTransactionDeclined += OnTransactionDeclined;

            void Web3Auth_OnLogout()
            {
                logoutTcs.SetResult(null);
            }
        }

        /// <summary>
        /// Signs a message using the private key associated with this wallet.
        /// </summary>
        /// <param name="message">The message to sign.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation and returns the signature as a string.</returns>
        public Task<string> SignMessage(string message) => signer.SignMessage(message);

        /// <summary>
        /// Signs an ERC2771 typed data request using the private key associated with this wallet.
        /// </summary>
        /// <typeparam name="TStructData">The type of the structured data to sign.</typeparam>
        /// <param name="domain">The domain information for the typed data.</param>
        /// <param name="message">The structured data to sign.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation and returns the signature as a string.</returns>
        public Task<string> SignTypedData<TStructData>(SerializableDomain domain, TStructData message) => signer.SignTypedData(domain, message);

        /// <summary>
        /// Sends a blockchain transaction using the private key associated with this wallet.
        /// </summary>
        /// <param name="transaction">The transaction request to send.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation and returns a <see cref="TransactionResponse"/>.</returns>
        public Task<TransactionResponse> SendTransaction(TransactionRequest transaction)
        {
            string id = Guid.NewGuid().ToString();

            var request = new TransactionRequested(id, transaction);

            var tcs = new TaskCompletionSource<TransactionResponse>();

            _transactionPool.Add(request, tcs);

            transactionHandler.RequestTransaction(request);

            return tcs.Task;
        }

        public async void OnTransactionApproved(TransactionApproved transactionApproved)
        {
            var pair = _transactionPool.Single(t => t.Key.Id == transactionApproved.Id);

            var response = await transactionExecutor.SendTransaction(pair.Key.Transaction);

            pair.Value.SetResult(response);

            transactionHandler.ConfirmTransaction(new TransactionConfirmed(response));

        }

        public void OnTransactionDeclined(TransactionDeclined transactionDeclined)
        {
            var pair = _transactionPool.Single(t => t.Key.Id == transactionDeclined.Id);

            pair.Value.SetCanceled();
        }

        /// <summary>
        /// Creates core instance.
        /// </summary>
        /// <returns>Core instance.</returns>
        private TWeb3Auth CreateCoreInstance()
        {
            Debug.Log("Creating Core Instance");
            var gameObject = new GameObject("Web3Auth", typeof(TWeb3Auth));
            Object.DontDestroyOnLoad(gameObject);

            var instance = gameObject.GetComponent<TWeb3Auth>();
            instance.setOptions(config.Web3AuthOptions, config.RememberMe);

            return instance;
        }
    }
}