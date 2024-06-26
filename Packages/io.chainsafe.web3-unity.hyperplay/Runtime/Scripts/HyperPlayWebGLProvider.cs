using System;
using System.Numerics;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm;
using ChainSafe.Gaming.Evm.Network;
using ChainSafe.Gaming.HyperPlay.Dto;
using ChainSafe.Gaming.LocalStorage;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Environment;
using Nethereum.Hex.HexTypes;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ChainSafe.Gaming.HyperPlay
{
    /// <summary>
    /// Concrete implementation of <see cref="HyperPlayProvider"/> for side-loaded browser games on HyperPlay desktop client.
    /// </summary>
    public class HyperPlayWebGLProvider : HyperPlayProvider
    {
        private readonly IHyperPlayConfig _config;
        private readonly IHyperPlayData _data;
        private readonly DataStorage _dataStorage;
        private readonly IChainConfig _chainConfig;
        private readonly ChainRegistryProvider _chainRegistryProvider;
        private readonly HyperPlayController _hyperPlayController;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="HyperPlayWebGLProvider"/> class.
        /// </summary>
        /// <param name="config">Injected <see cref="HyperPlayConfig"/>.</param>
        /// <param name="data">Injected <see cref="IHyperPlayData"/>.</param>
        /// <param name="dataStorage">Injected <see cref="DataStorage"/>.</param>
        /// <param name="httpClient">HttpClient to make requests.</param>
        /// <param name="chainConfig">ChainConfig to fetch chain data.</param>
        /// <param name="chainRegistryProvider">Injected <see cref="ChainRegistryProvider"/>.</param>
        public HyperPlayWebGLProvider(IHyperPlayConfig config, IHyperPlayData data, DataStorage dataStorage, IHttpClient httpClient, IChainConfig chainConfig, ChainRegistryProvider chainRegistryProvider) : base(config, data, dataStorage, httpClient, chainConfig, chainRegistryProvider)
        {
            _config = config;
            _data = data;
            _dataStorage = dataStorage;
            _chainConfig = chainConfig;
            _chainRegistryProvider = chainRegistryProvider;
            
            // Initialize Unity controller.
            _hyperPlayController = Object.FindObjectOfType<HyperPlayController>();

            if (_hyperPlayController == null)
            {
                GameObject controllerObj = new GameObject(nameof(HyperPlayController), typeof(HyperPlayController));

                _hyperPlayController = controllerObj.GetComponent<HyperPlayController>();
            }

            Object.DontDestroyOnLoad(_hyperPlayController.gameObject);
        }

        /// <summary>
        /// Connect to wallet from a side-loaded browser game via HyperPlay desktop client and return the account address.
        /// </summary>
        /// <returns>Signed-in account public address.</returns>
        public override async Task<string> Connect()
        {
            string[] accounts = await Perform<string[]>("eth_requestAccounts");

            string account = accounts[0];

            string currentChainId = await Perform<string>("eth_chainId");

            var currentIntegerChainId = (new HexBigInteger(currentChainId)).Value;

            int intChainId = int.Parse(_chainConfig.ChainId);
            
            // If chain is different change it.
            if (currentIntegerChainId != intChainId)
            {
                string hexChainId = (new BigInteger(intChainId)).ToHexBigInteger().HexValue;
                
                try
                {
                    await Perform<string>("wallet_switchEthereumChain", new Chain
                    {
                        ChainId = hexChainId
                    });
                }
                catch (Exception)
                {
                    var chain = await _chainRegistryProvider.GetChain((ulong) intChainId);

                    var nativeCurrency = chain?.NativeCurrencyInfo;
                    
                    await Perform<string>("wallet_addEthereumChain", new Chain
                    {
                        ChainId = hexChainId,
                        Name = chain != null ? chain.Name : _chainConfig.Chain,
                        RpcUrls = chain != null ? chain.RPC : new string[]{_chainConfig.Rpc},
                        NativeCurrency = chain != null ? new NativeCurrency
                        {
                            Name = nativeCurrency.Name,
                            Symbol = nativeCurrency.Symbol,
                            Decimals = (int) nativeCurrency.Decimals,
                        } : new NativeCurrency(_chainConfig.Symbol),
                    });
                }
            }
            
            // Saved account exists.
            if (_data.RememberSession)
            {
                return account;
            }

            if (_config.RememberSession)
            {
                _data.RememberSession = true;

                _data.SavedAccount = account;

                await _dataStorage.Save(_data);
            }

            return account;
        }

        /// <summary>
        /// Make RPC request on HyperPlay desktop client.
        /// </summary>
        /// <param name="method">RPC request method name.</param>
        /// <param name="parameters">RPC request parameters.</param>
        /// <typeparam name="T">RPC request response type.</typeparam>
        /// <returns>RPC request Response.</returns>
        public override async Task<T> Perform<T>(string method, params object[] parameters)
        {
            var response = await _hyperPlayController.Request(method, parameters);
            
            return response.Result.ToObject<T>();
        }
    }
}
