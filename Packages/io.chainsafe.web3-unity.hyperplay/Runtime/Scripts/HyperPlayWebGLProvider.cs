#if UNITY_WEBGL && !UNITY_EDITOR
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm;
using ChainSafe.Gaming.LocalStorage;
using ChainSafe.Gaming.Unity.EthereumWindow;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Environment;
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
        private readonly ILocalStorage _localStorage;
        private readonly IChainConfig _chainConfig;
        private readonly ChainRegistryProvider _chainRegistryProvider;
        private readonly EthereumWindowController _ethereumController;

        /// <summary>
        /// Initializes a new instance of the <see cref="HyperPlayWebGLProvider"/> class.
        /// </summary>
        /// <param name="config">Injected <see cref="IHyperPlayConfig"/>.</param>
        /// <param name="data">Injected <see cref="IHyperPlayData"/>.</param>
        /// <param name="localStorage">Injected <see cref="ILocalStorage"/>.</param>
        /// <param name="environment">Injected <see cref="Web3Environment"/>.</param>
        /// <param name="chainConfig">ChainConfig to fetch chain data.</param>
        /// <param name="chainRegistryProvider">Injected <see cref="ChainRegistryProvider"/>.</param>
        public HyperPlayWebGLProvider(IHyperPlayConfig config, IHyperPlayData data, ILocalStorage localStorage, Web3Environment environment, IChainConfig chainConfig, ChainRegistryProvider chainRegistryProvider) : base(config, data, localStorage, environment, chainConfig)
        {
            _config = config;
            _data = data;
            _localStorage = localStorage;
            _chainConfig = chainConfig;
            _chainRegistryProvider = chainRegistryProvider;

            // Initialize Unity controller.
            _ethereumController = Object.FindObjectOfType<EthereumWindowController>();

            if (_ethereumController == null)
            {
                GameObject controllerObj = new GameObject(nameof(EthereumWindowController), typeof(EthereumWindowController));

                _ethereumController = controllerObj.GetComponent<EthereumWindowController>();
            }

            Object.DontDestroyOnLoad(_ethereumController.gameObject);
        }

        /// <summary>
        /// Connect to wallet from a side-loaded browser game via HyperPlay desktop client and return the account address.
        /// </summary>
        /// <returns>Signed in account public address.</returns>
        public override async Task<string> Connect()
        {
            string account = await _ethereumController.Connect(_chainConfig, _chainRegistryProvider);

            // Saved account exists.
            if (_data.RememberSession)
            {
                return account;
            }

            if (_config.RememberSession)
            {
                _data.RememberSession = true;

                _data.SavedAccount = account;

                await _localStorage.Save(_data);
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
        public override async Task<T> Request<T>(string method, params object[] parameters)
        {
            var response = await _ethereumController.Request(method, parameters);

            return response.Result.ToObject<T>();
        }
    }
}

#endif