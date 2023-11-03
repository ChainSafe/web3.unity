using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Providers;
using ChainSafe.Gaming.Evm.Signers;
using ChainSafe.Gaming.Evm.Transactions;
using ChainSafe.Gaming.InProcessSigner;
using ChainSafe.Gaming.InProcessTransactionExecutor;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Core;
using ChainSafe.Gaming.Web3.Core.Evm;
using Nethereum.Signer;
using UnityEngine;
using TWeb3Auth = Web3Auth;

namespace ChainSafe.GamingSdk.Web3Auth
{
    /// <summary>
    /// A class that represents a Web3Auth wallet and provides functionality for signing transactions and interacting with a blockchain.
    /// </summary>
    public class Web3AuthWallet : ISigner, ITransactionExecutor, ILifecycleParticipant
    {
        private readonly IChainConfig chainConfig;

        private readonly Web3AuthWalletConfig config;
        private readonly IRpcProvider rpcProvider;
        private TWeb3Auth coreInstance;
        private InProcessSigner signer;
        private InProcessTransactionExecutor transactionExecutor;

        /// <summary>
        /// Initializes a new instance of the <see cref="Web3AuthWallet"/> class.
        /// </summary>
        /// <param name="config">The configuration for the Web3Auth wallet.</param>
        /// <param name="chainConfig">The configuration for the target blockchain.</param>
        /// <param name="rpcProvider">The RPC provider for blockchain interaction.</param>
        public Web3AuthWallet(Web3AuthWalletConfig config, IChainConfig chainConfig, IRpcProvider rpcProvider)
        {
            this.config = config;
            this.chainConfig = chainConfig;
            this.rpcProvider = rpcProvider;
        }

        /// <summary>
        /// Asynchronously prepares the Web3Auth wallet for operation, triggered when initializing the module in the dependency injection work flow.
        /// </summary>
        /// <returns>A <see cref="ValueTask"/> representing the asynchronous operation.</returns>
        public async ValueTask WillStartAsync()
        {
            coreInstance = CreateCoreInstance();
            TaskCompletionSource<string> loginTcs = new();
            coreInstance.onLogin += Web3Auth_OnLogin;

            if (config.LoginParams != null) coreInstance.login(config.LoginParams);

            var privateKeyString = await loginTcs.Task;

            var privateKey = new EthECKey(privateKeyString);
            var signerConfig = new InProcessSignerConfig { PrivateKey = privateKey };
            signer = new InProcessSigner(signerConfig);

            transactionExecutor = new InProcessTransactionExecutor(signer, chainConfig, rpcProvider);

            void Web3Auth_OnLogin(Web3AuthResponse response)
            {
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
            Object.Destroy(coreInstance);

            void Web3Auth_OnLogout()
            {
                logoutTcs.SetResult(null);
            }
        }

        /// <summary>
        /// Gets the blockchain address associated with this wallet.
        /// </summary>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation and returns the blockchain address as a string.</returns>
        public Task<string> GetAddress() => signer.GetAddress();

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
        public Task<TransactionResponse> SendTransaction(TransactionRequest transaction) => transactionExecutor.SendTransaction(transaction);

        private TWeb3Auth CreateCoreInstance()
        {
            var gameObject = new GameObject("Web3Auth", typeof(TWeb3Auth));
            gameObject.hideFlags = HideFlags.HideInHierarchy;
            Object.DontDestroyOnLoad(gameObject);

            var instance = gameObject.GetComponent<TWeb3Auth>();
            instance.Initialize();
            instance.setOptions(config.Web3AuthOptions);

            return instance;
        }
    }
}