using System.Threading.Tasks;
using ChainSafe.Gaming.Evm;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Analytics;
using ChainSafe.Gaming.Web3.Environment;
using ChainSafe.Gaming.Web3.Evm.Wallet;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ChainSafe.Gaming.Unity.MetaMask
{
    /// <summary>
    /// Concrete implementation of <see cref="MetaMaskProvider"/>.
    /// </summary>
    public class MetaMaskProvider : WalletProvider
    {
        private readonly ILogWriter logWriter;

        private readonly MetaMaskController metaMaskController;
        private readonly IChainConfig chainConfig;
        private readonly IAnalyticsClient analyticsClient;
        private readonly ChainRegistryProvider chainRegistryProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="MetaMaskProvider"/> class.
        /// </summary>
        /// <param name="environment">Injected <see cref="Web3Environment"/>.</param>
        /// <param name="chainConfig">Injected <see cref="IChainConfig"/>.</param>
        /// <param name="chainRegistryProvider">Injected <see cref="ChainRegistryProvider"/>.</param>
        public MetaMaskProvider(Web3Environment environment, IChainConfig chainConfig, ChainRegistryProvider chainRegistryProvider)
            : base(environment, chainConfig)
        {
            logWriter = environment.LogWriter;
            this.chainConfig = chainConfig;
            analyticsClient = environment.AnalyticsClient;
            this.chainRegistryProvider = chainRegistryProvider;

            if (Application.isEditor || Application.platform != RuntimePlatform.WebGLPlayer)
            {
                logWriter.LogError("You need to build to WebGL platform to run Nethereum.Metamask.Unity");

                return;
            }

            // Initialize Unity controller.
            metaMaskController = Object.FindObjectOfType<MetaMaskController>();

            if (metaMaskController == null)
            {
                GameObject controllerObj = new GameObject(nameof(MetaMaskController), typeof(MetaMaskController));

                metaMaskController = controllerObj.GetComponent<MetaMaskController>();
            }

            Object.DontDestroyOnLoad(metaMaskController.gameObject);
        }

        public override Task Disconnect()
        {
            Object.Destroy(metaMaskController.gameObject);

            return Task.CompletedTask;
        }

        public override async Task<T> Request<T>(string method, params object[] parameters)
        {
            var response = await metaMaskController.Request(method, parameters);

            return response.Result.ToObject<T>();
        }

        /// <summary>
        /// Implementation of <see cref="IWalletProvider.Connect"/>.
        /// Called to connect to MetaMask.
        /// </summary>
        /// <returns>Connected account.</returns>
        public override async Task<string> Connect()
        {
            logWriter.Log("Connecting from Metamask...");

            analyticsClient.CaptureEvent(new AnalyticsEvent()
            {
                EventName = "Metamask WebGL Initialized",
                PackageName = "io.chainsafe.web3-unity",
            });

            return await metaMaskController.Connect(chainConfig, chainRegistryProvider);
        }
    }
}