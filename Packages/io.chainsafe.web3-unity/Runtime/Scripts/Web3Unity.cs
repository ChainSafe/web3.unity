using System;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Contracts;
using ChainSafe.Gaming.Evm.Contracts.Extensions;
using ChainSafe.Gaming.Evm.Providers;
using ChainSafe.Gaming.Evm.Signers;
using ChainSafe.Gaming.Evm.Transactions;
using ChainSafe.Gaming.UnityPackage.Connection;
using ChainSafe.Gaming.UnityPackage.UI;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Core.Evm;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Signer;
using Nethereum.Util;
using Newtonsoft.Json;
using UnityEngine;
using CWeb3 = ChainSafe.Gaming.Web3.Web3;
using TransactionReceipt = ChainSafe.Gaming.Evm.Transactions.TransactionReceipt;

namespace ChainSafe.Gaming.UnityPackage
{
    /// <summary>
    /// Component for managing Web3 connection and operations.
    /// </summary>
    [RequireComponent(typeof(ConnectionHandler))]
    public class Web3Unity : MonoBehaviour, IWeb3InitializedHandler
    {
        public static bool TestMode = false;
        
        private static Web3Unity _instance;

        /// <summary>
        /// Static Web3 singleton instance.
        /// </summary>
        public static Web3Unity Instance
        {
            get
            {
                if (!_instance) _instance = FindObjectOfType<Web3Unity>();

                return _instance;
            }
        }

        public static event Action<(CWeb3 web3, bool isLightweight)> Web3Initialized;

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
        public static bool Connected => !string.IsNullOrEmpty(Instance.PublicAddress);

        /// <summary>
        /// Public key (address) of connected wallet.
        /// </summary>
        public string PublicAddress
        {
            get
            {
                try
                {
                    return Web3?.Signer.PublicAddress;
                }
                catch (Exception)
                {
                    return string.Empty;
                }
            }
        }

        [DefaultAssetValue("Packages/io.chainsafe.web3-unity/Runtime/Prefabs/Connect.prefab")]
        [SerializeField]
        private ConnectModal connectModalPrefab;

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
        /// <param name="rememberConnection">Connect to any saved <see cref="ConnectionProvider"/> if they exist.</param>
        public async Task Initialize(bool rememberConnection = true)
        {
            _connectionHandler = GetComponent<ConnectionHandler>();

            await _connectionHandler.Initialize(rememberConnection);

            if (rememberConnection)
            {
                try
                {
                    await _connectionHandler.Restore();

                    if (Connected)
                    {
                        return;
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError($"Failed to restore connection: {e}");
                }
            }

            await LaunchLightWeightWeb3();
        }

        private Task LaunchLightWeightWeb3()
        {
            return ((IConnectionHandler)_connectionHandler).LaunchLightWeightWeb3();
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
                throw new Web3Exception(
                    $"{typeof(T).Name} unavailable or disabled. Check under Connection Providers in {nameof(ConnectionHandler)}.");

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

        public async Task<HexBigInteger> GetGasPrice()
        {
            return await Web3.RpcProvider.GetGasPrice();
        }

        public async Task<HexBigInteger> GetBlockNumber()
        {
            return await Web3.RpcProvider.GetBlockNumber();
        }

        public async Task<object[]> ContractSend(string method, string abi, string contractAddress, object[] args,
            HexBigInteger value = null)
        {
            var contract = Web3.ContractBuilder.Build(abi, contractAddress);
            var overwrite = value != null ? new TransactionRequest { Value = value } : null;
            return await contract.Send(method, args, overwrite);
        }

        public async Task<object[]> ContractCall(string method, string abi, string contractAddress, object[] args)
        {
            var contract = Web3.ContractBuilder.Build(abi, contractAddress);
            return await contract.Call(method, args);
        }

        public async Task<HexBigInteger> GetGasLimit(string contractAbi, string contractAddress, string method,
            object[] args)
        {
            var contract = Web3.ContractBuilder.Build(contractAbi, contractAddress);
            return await contract.EstimateGas(method, args);
        }

        public T[] GetEvents<T>(TransactionReceipt receipt) where T : IEventDTO, new()
        {
            var logs = receipt.Logs.Select(jToken => JsonConvert.DeserializeObject<FilterLog>(jToken.ToString()));
            var eventAbi = EventExtensions.GetEventABI<T>();
            var events = logs
                .Select(log => eventAbi.DecodeEvent<T>(log))
                .Where(l => l != null).Select(x => x.Event).ToArray();
            return events;
        }

        public string GetPublicAddressFromPrivateKey(string privateKey)
        {
            return new EthECKey(privateKey).GetPublicAddress();
        }

        public async Task<string> SendTransaction(string to, BigInteger value)
        {
            var txRequest = new TransactionRequest
            {
                To = to,
                Value = new HexBigInteger(value.ToString("X")),
                MaxFeePerGas = new HexBigInteger((await Web3.RpcProvider.GetFeeData()).MaxFeePerGas)
            };
            var response = await Web3.TransactionExecutor.SendTransaction(txRequest);
            return response.Hash;
        }

        public string SignMessageWithPrivateKey(string privateKey, string message)
        {
            var signer = new EthereumMessageSigner();
            var signature = signer.HashAndSign(message, privateKey);
            return signature;
        }

        public async Task OnWeb3Initialized(CWeb3 web3)
        {
            // Terminate if there's any existing Web3 Instance
            await Terminate(false);
            
            _web3 = web3;

            if (_connectModal != null)
            {
                _connectModal.Close();
            }
            
            Web3Initialized?.Invoke((_web3, _web3.ServiceProvider.GetService(typeof(ISigner)) == null));
        }

        public async Task<TransactionResponse> GetTransactionByHash(string transactionHash)
        {
            if (Web3 == null || Web3.RpcProvider == default)
                throw new InvalidOperationException(
                    "Web3 object and/or the RPC provider are null. Make sure you've initialized the Web3 object correctly ");
            var parameters = new object[] { transactionHash };
            var transaction =
                await Web3.RpcProvider.Perform<TransactionResponse>("eth_getTransactionByHash", parameters);

            if (transaction == null) throw new Web3Exception("Transaction not found.");

            if (transaction.BlockNumber == null)
            {
                transaction.Confirmations = 0;
            }
            else if (transaction.Confirmations == null)
            {
                var blockNumber = await Web3.RpcProvider.GetBlockNumber();
                var confirmations = blockNumber.ToUlong() - transaction.BlockNumber.ToUlong() + 1;
                if (confirmations <= 0) confirmations = 1;

                transaction.Confirmations = confirmations;
            }

            return transaction;
        }

        private static void AssertConnection()
        {
            if (!Connected) throw new Web3Exception("Account not connected.");
        }

        /// <summary>
        /// Disconnect wallet.
        /// </summary>
        /// <returns>Awaitable disconnect task.</returns>
        public async Task Disconnect()
        {
            await Terminate(true);

            await LaunchLightWeightWeb3();
        }

        public async Task<bool> SignAndVerifyMessage(string message)
        {
            var playerAccount = Web3.Signer.PublicAddress;
            var signatureString = await Web3.Signer.SignMessage(message);
            var msg = "\x19" + "Ethereum Signed Message:\n" + message.Length + message;
            var msgHash = new Sha3Keccack().CalculateHash(Encoding.UTF8.GetBytes(msg));
            var signature = MessageSigner.ExtractEcdsaSignature(signatureString);
            var key = EthECKey.RecoverFromSignature(signature, msgHash);
            return string.Equals(key.GetPublicAddress(), playerAccount, StringComparison.CurrentCultureIgnoreCase);
        }

        public string GetMessageHash(string message)
        {
            return new Sha3Keccack().CalculateHash(message);
        }

        /// <summary>
        /// Terminate Web3 instance.
        /// </summary>
        /// <param name="logout">Is Logout.</param>
        private async Task Terminate(bool logout)
        {
            if (Web3 != null)
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