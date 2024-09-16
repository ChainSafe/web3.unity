using System;
using System.Numerics;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Contracts;
using ChainSafe.Gaming.Evm.Providers;
using ChainSafe.Gaming.Evm.Transactions;
using ChainSafe.Gaming.UnityPackage.Connection;
using ChainSafe.Gaming.UnityPackage.UI;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Core.Evm;
using Nethereum.Hex.HexTypes;
using UnityEngine;
using CWeb3 = ChainSafe.Gaming.Web3.Web3;

namespace ChainSafe.Gaming.UnityPackage
{
    /// <summary>
    /// Component for managing Web3 connection and operations.
    /// </summary>
    [RequireComponent(typeof(ConnectionHandler))]
    public class Web3Unity : MonoBehaviour, IWeb3InitializedHandler
    {
        private static Web3Unity _instance;
        
        /// <summary>
        /// Static Web3 singleton instance.
        /// </summary>
        public static Web3Unity Instance
        {
            get
            {
                if (!_instance)
                {
                    _instance = FindObjectOfType<Web3Unity>();
                }
                
                return _instance;
            }
        }

        /// <summary>
        /// Web3 Instance.
        /// </summary>
        public static CWeb3 Web3 => Instance?._web3;

        /// <summary>
        /// Connection Modal used to connect to available <see cref="ConnectionProvider"/>s.
        /// </summary>
        public static ConnectModal ConnectModal
        {
            get
            {
                if (!Instance._connectModal)
                {
                    Instance._connectModal = Instantiate(Instance.connectModalPrefab);
                    
                    Instance._connectModal.Initialize(Instance._connectionHandler.Providers);
                }

                return Instance._connectModal;
            }
        }

        /// <summary>
        /// Is a wallet connected.
        /// </summary>
        public static bool Connected => Web3 != null;
        
        /// <summary>
        /// Execution priority for <see cref="IWeb3InitializedHandler"/>.
        /// Lower than other so it can be executed first.
        /// </summary>
        public int Priority => - 1;
        
        /// <summary>
        /// Public key (address) of connected wallet.
        /// </summary>
        public string Address => Web3?.Signer.PublicAddress;
        
        [DefaultAssetValue("Packages/io.chainsafe.web3-unity/Runtime/Prefabs/Connect.prefab")]
        [SerializeField] private ConnectModal connectModalPrefab;
        
        private CWeb3 _web3;
        
        private ConnectionHandler _connectionHandler;
        
        private ConnectModal _connectModal;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        /// <summary>
        /// Initialize Web3Unity.
        /// </summary>
        /// <param name="connectOnInitialize">Connect to any saved <see cref="ConnectionProvider"/> if they exist.</param>
        public async Task Initialize(bool connectOnInitialize = true)
        {
            _connectionHandler = GetComponent<ConnectionHandler>();
            
            await _connectionHandler.Initialize();

            if (connectOnInitialize)
            {
                try
                {
                    await _connectionHandler.Restore();
                }
                catch (Exception e)
                {
                    Debug.LogError($"Failed to restore connection: {e}");
                }
            }
        }

        /// <summary>
        /// Connect to a wallet with a <see cref="ConnectionProvider"/>.
        /// </summary>
        /// <param name="provider"><see cref="ConnectionProvider"/> used to connect to wallet.</param>
        public async Task Connect(ConnectionProvider provider)
        {
            await (_connectionHandler as IConnectionHandler).Connect(provider);
        }
        
        /// <summary>
        /// Connect to a wallet with a <see cref="ConnectionProvider"/>.
        /// </summary>
        /// <typeparam name="T">Type of <see cref="ConnectionProvider"/>.</typeparam>
        /// <exception cref="Web3Exception">Exception thrown if <see cref="ConnectionProvider"/> of Type <see cref="T"/> is not available or disabled in <see cref="ConnectionHandler"/>.</exception>
        public async Task Connect<T>() where T : ConnectionProvider
        {
            if (!_connectionHandler.GetProvider(out T provider))
            {
                throw new Web3Exception($"{typeof(T).Name} unavailable or disabled. Check under Connection Providers in {nameof(ConnectionHandler)}.");
            }
            
            await Connect(provider);
        }

        /// <summary>
        /// Sign message.
        /// </summary>
        /// <param name="message">Message to be signed.</param>
        /// <returns>Signature hash.</returns>
        public Task<string> SignMessage(string message)
        {
            return Web3.Signer.SignMessage(message);
        }

        /// <summary>
        /// Sign Typed Data.
        /// </summary>
        /// <typeparam name="TStructType">Type of data sign.</typeparam>
        /// <param name="domain">The domain parameters for signing.</param>
        /// <param name="message">Typed message to sign.</param>
        /// <returns>Signature hash.</returns>
        public Task<string> SignTypedData<TStructType>(SerializableDomain domain, TStructType message)
        {
            return Web3.Signer.SignTypedData(domain, message);
        }
        
        /// <summary>
        /// Build Custom contracts.
        /// </summary>
        /// <param name="address">Contract address.</param>
        /// <typeparam name="T">Type of custom contract.</typeparam>
        /// <returns>Built custom contract type.</returns>
        public Task<T> BuildContract<T>(string address) where T : ICustomContract, new()
        {
            return Web3.ContractBuilder.Build<T>(address);
        }
        
        public Task OnWeb3Initialized(CWeb3 web3)
        {
            _web3 = web3;
            
            return Task.CompletedTask;
        }

        public async Task<TransactionResponse> GetTransactionByHash(string transactionHash)
        {
            if (Web3 == null || _web3.RpcProvider == default)
            {
                throw new InvalidOperationException("Web3 object and/or the RPC provider are null. Make sure you've initialized the Web3 object correctly ");
            }
            var parameters = new object[] { transactionHash };
            TransactionResponse transaction = await Web3.RpcProvider.Perform<TransactionResponse>("eth_getTransactionByHash", parameters);
            
            if (transaction == null)
            {
                throw new Web3Exception("Transaction not found.");
            }

            if (transaction.BlockNumber == null)
            {
                transaction.Confirmations = 0;
            }
            else if (transaction.Confirmations == null)
            {
                var blockNumber = await Web3.RpcProvider.GetBlockNumber();
                var confirmations = (blockNumber.ToUlong() - transaction.BlockNumber.ToUlong()) + 1;
                if (confirmations <= 0)
                {
                    confirmations = 1;
                }

                transaction.Confirmations = confirmations;
            }

            return transaction;
        }
        
        private static void AssertConnection()
        {
            if (!Connected)
            {
                throw new Web3Exception("Account not connected.");
            }
        }
        
        /// <summary>
        /// Disconnect wallet.
        /// </summary>
        /// <returns>Awaitable disconnect task.</returns>
        public Task Disconnect()
        {
            return Terminate(true);
        }
        
        /// <summary>
        /// Terminate Web3 instance.
        /// </summary>
        /// <param name="logout">Is Logout.</param>
        private async Task Terminate(bool logout)
        {
            if (Connected)
            {
                await Web3.TerminateAsync(logout);

                _web3 = null;
            }
        }

        private async void OnDestroy()
        {
            // Terminate Web3 instance
            await Terminate(false);
        }
    }
}
