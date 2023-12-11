using System.Threading.Tasks;
using ChainSafe.Gaming.Web3.Environment;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ChainSafe.Gaming.MetaMask.Unity
{
    /// <summary>
    /// Concrete implementation of <see cref="IMetaMaskProvider"/>.
    /// </summary>
    public class MetaMaskProvider : IMetaMaskProvider
    {
        private readonly ILogWriter logWriter;

        private readonly MetaMaskController metaMaskController;

        /// <summary>
        /// Initializes a new instance of the <see cref="MetaMaskProvider"/> class.
        /// </summary>
        /// <param name="logWriter">Common Logger used for logging messages and errors.</param>
        public MetaMaskProvider(ILogWriter logWriter)
        {
            this.logWriter = logWriter;

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

        /// <summary>
        /// Implementation of <see cref="IMetaMaskProvider.Connect"/>.
        /// Called to connect to MetaMask.
        /// </summary>
        /// <returns>Connected account.</returns>
        public async Task<string> Connect()
        {
            logWriter.Log("Connecting from Metamask...");

            return await metaMaskController.Connect();
        }

        /// <summary>
        /// Make JsonRPC requests using MetaMask.
        /// </summary>
        /// <param name="method">JsonRPC method name.</param>
        /// <param name="parameters">JsonRPC request parameters.</param>
        /// <typeparam name="T">Type of response result.</typeparam>
        /// <returns>Response result.</returns>
        public async Task<T> Request<T>(string method, params object[] parameters)
        {
            return await metaMaskController.Request<T>(method, parameters);
        }
    }
}