using System.Threading.Tasks;
using ChainSafe.Gaming.Evm;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Analytics;
using ChainSafe.Gaming.Web3.Core.Logout;
using ChainSafe.Gaming.Web3.Environment;
using ChainSafe.Gaming.Web3.Evm.Wallet;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ChainSafe.Gaming.MetaMask.Unity
{
    /// <summary>
    /// Concrete implementation of <see cref="IMetaMaskProvider"/>.
    /// </summary>
    public class MetaMaskProvider : WalletProvider, ILogoutHandler
    {
        private readonly ILogWriter logWriter;

        private readonly MetaMaskController metaMaskController;
        private readonly IChainConfig chainConfig;
        private readonly IAnalyticsClient analyticsClient;
        private readonly IProjectConfig projectConfig;

        /// <summary>
        /// Initializes a new instance of the <see cref="MetaMaskProvider"/> class.
        /// </summary>
        /// <param name="logWriter">Common Logger used for logging messages and errors.</param>
        /// <param name="chainConfig">Injected <see cref="IChainConfig"/>.</param>
        /// <param name="projectConfig">Injected <see cref="IProjectConfig"/>.</param>
        /// <param name="chainRegistryProvider">Injected <see cref="ChainRegistryProvider"/>.</param>
        /// <param name="analyticsClient">Injected <see cref="IAnalyticsClient"/>.</param>
        public MetaMaskProvider(ILogWriter logWriter, IAnalyticsClient analyticsClient, IChainConfig chainConfig, IProjectConfig projectConfig, ChainRegistryProvider chainRegistryProvider)
            : base(
            chainRegistryProvider: chainRegistryProvider)
        {
            this.logWriter = logWriter;
            this.chainConfig = chainConfig;
            this.analyticsClient = analyticsClient;
            this.projectConfig = projectConfig;

            if (Application.isEditor || Application.platform != RuntimePlatform.WebGLPlayer)
            {
                this.logWriter.LogError("You need to build to WebGL platform to run Nethereum.Metamask.Unity");

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

            metaMaskController.Initialize(this.logWriter);
        }

        public override Task Disconnect()
        {
            // Currently no disconnect logic for MetaMask lib on NEthereum.
            return Task.CompletedTask;
        }

        public override async Task<T> Perform<T>(string method, params object[] parameters)
        {
            return await metaMaskController.Request<T>(method, parameters);
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

            return await metaMaskController.Connect();
        }

        public Task OnLogout()
        {
            Object.Destroy(metaMaskController.gameObject);

            return Task.CompletedTask;
        }
    }
}