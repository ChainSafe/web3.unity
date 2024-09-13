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
    [RequireComponent(typeof(ConnectionHandler))]
    public class Web3Unity : MonoBehaviour, IWeb3InitializedHandler
    {
        private static Web3Unity _instance;
        
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

        public static CWeb3 Web3 => Instance._web3;

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

        public static bool Connected => Web3 != null;
        
        public int Priority => - 1;
        
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

        public async Task Connect(ConnectionProvider provider)
        {
            await (_connectionHandler as IConnectionHandler).Connect(provider);
        }
        
        public async Task Connect<T>() where T : ConnectionProvider
        {
            if (!_connectionHandler.GetProvider(out T provider))
            {
                throw new Web3Exception($"{typeof(T).Name} unavailable or disabled.");
            }
            
            await Connect(provider);
        }

        public Task<string> SignMessage(string message)
        {
            return Web3.Signer.SignMessage(message);
        }

        public Task<string> SignTypedData<TStructType>(SerializableDomain domain, TStructType message)
        {
            return Web3.Signer.SignTypedData(domain, message);
        }
        
        public async Task<string> SendTransaction(string toAddress, BigInteger value)
        {
            var transaction = new TransactionRequest
            {
                From = Address,
                To = toAddress,
                Value = new HexBigInteger(value.ToString("X")),
                MaxFeePerGas = (await Web3.RpcProvider.GetFeeData()).MaxFeePerGas.ToHexBigInteger()
            };
            
            var response = await Web3.TransactionExecutor.SendTransaction(transaction);
            
            return response.Hash;
        }

        public Task<T> Build<T>(string address) where T : ICustomContract, new()
        {
            return Web3.ContractBuilder.Build<T>(address);
        }
        
        public Task OnWeb3Initialized(CWeb3 web3)
        {
            _web3 = web3;
            
            return Task.CompletedTask;
        }
        
        private static void AssertConnection()
        {
            if (!Connected)
            {
                throw new Web3Exception("Account not connected.");
            }
        }
        
        public Task Disconnect()
        {
            return Terminate(true);
        }
        
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
