using System.Collections;
using System.Threading.Tasks;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Environment;
using Nethereum.Hex.HexTypes;
using Nethereum.JsonRpc.Client;
using Nethereum.Unity.Metamask;
using Nethereum.Unity.Rpc;
using UnityEngine;

namespace ChainSafe.Gaming.MetaMask.Unity
{
    /// <summary>
    /// Controller script for connecting to MetaMask and making awaitable JsonRPC requests.
    /// </summary>
    public class MetaMaskController : MonoBehaviour
    {
        private bool isInitialized;

        /// Use this instead of <see cref="Debug"/>.
        private ILogWriter logger;

        /// <summary>
        /// Delegate for when MetaMask account is connected.
        /// </summary>
        public delegate void AccountConnected(string address);

        /// <summary>
        /// Delegate for when MetaMask JsonRPC request gets a callback.
        /// </summary>
        /// <typeparam name="T">Type for the response's result.</typeparam>
        public delegate void ResponseCallback<T>(T result);

        /// <summary>
        /// Event invoked when MetaMask account is connected.
        /// </summary>
        public event AccountConnected OnAccountConnected;

        /// <summary>
        /// Address of successfully connected account.
        /// </summary>
        public string ConnectedAddress { get; private set; }

        /// <summary>
        /// Initialize script with references.
        /// </summary>
        /// <param name="logWriter">Use to write logs. Use this instead of <see cref="Debug"/>.</param>
        public void Initialize(ILogWriter logWriter)
        {
            logger = logWriter;
        }

        /// <summary>
        /// Awaitable method that Connects to MetaMask.
        /// </summary>
        /// <returns>Connected account address.</returns>
        public Task<string> Connect()
        {
            var taskCompletionSource = new TaskCompletionSource<string>();

            // Unsubscribe in case we're already subscribed from a previous login.
            OnAccountConnected -= Connected;

            OnAccountConnected += Connected;

            // Callback for when we successfully connect.
            void Connected(string address)
            {
                ConnectedAddress = address;

                if (!taskCompletionSource.TrySetResult(ConnectedAddress))
                {
                    taskCompletionSource.SetException(new Web3Exception("Error setting connected account address."));
                }
                else
                {
                    logger.Log($"MetaMask successfully connected to address {ConnectedAddress}");
                }
            }

            // Connect to MetaMask.
            if (MetamaskWebglInterop.IsMetamaskAvailable())
            {
                MetamaskWebglInterop.EnableEthereum(gameObject.name, nameof(EthereumEnabled), nameof(DisplayError));
            }
            else
            {
                logger.LogError("Metamask is not available, please install it first.");

                // Unsubscribe to event.
                OnAccountConnected -= Connected;

                return null;
            }

            return taskCompletionSource.Task;
        }

        /// <summary>
        /// Awaitable Request method for making JsonRPC requests using MetaMask.
        /// </summary>
        /// <param name="method">JsonPRC method name.</param>
        /// <param name="parameters">JsonRPC parameters.</param>
        /// <typeparam name="T">Type for returned value on response.</typeparam>
        /// <returns>Response result with type <see cref="T"/>.</returns>
        public Task<T> Request<T>(string method, params object[] parameters)
        {
            var taskCompletionSource = new TaskCompletionSource<T>();

            void ResponseCallback(T result)
            {
                if (!taskCompletionSource.TrySetResult(result))
                {
                    taskCompletionSource.SetException(new Web3Exception("Error setting response result."));
                }
            }

            StartCoroutine(Request<T>(new RpcRequest(Configuration.DefaultRequestId, method, parameters), ResponseCallback));

            return taskCompletionSource.Task;
        }

        private IEnumerator Request<T>(RpcRequest data, ResponseCallback<T> onResponse)
        {
            var rpcRequest = new UnityRpcRequest<T>(new MetamaskWebglCoroutineRequestRpcClientFactory(ConnectedAddress));

            yield return rpcRequest.SendRequest(data);

            if (rpcRequest.Exception != null)
            {
                logger.LogError($"MetaMask Exception while making {data.Method} Json RPC request {rpcRequest.Exception.Message}");

                // Even if request fails we still need to callback so request task completion source result can be set.
                onResponse?.Invoke(default);

                throw rpcRequest.Exception;
            }

            onResponse?.Invoke(rpcRequest.Result);
        }

        /// <summary>
        /// Callback when MetaMask successfully connects to an account.
        /// </summary>
        /// <param name="address">Connected Address.</param>
        public void EthereumEnabled(string address)
        {
            if (!isInitialized)
            {
                // Subscribe to callbacks.
                MetamaskWebglInterop.EthereumInit(gameObject.name, nameof(NewAccountSelected), nameof(ChainChanged));

                // Get and log ChainId.
                MetamaskWebglInterop.GetChainId(gameObject.name, nameof(ChainSelected), nameof(DisplayError));

                isInitialized = true;
            }

            InvokeAccountConnected(address);
        }

        private void InvokeAccountConnected(string address)
        {
            OnAccountConnected?.Invoke(address);
        }

        /// <summary>
        /// Callback when a new account is selected by user.
        /// </summary>
        /// <param name="address">New selected account address.</param>
        public void NewAccountSelected(string address)
        {
            throw new Web3Exception($"{nameof(ChainSafe)} SDK doesn't support account switching during a single session.");
        }

        /// <summary>
        /// Callback when chain is changed on Network.
        /// </summary>
        /// <param name="chainId">New chain.</param>
        public void ChainChanged(string chainId)
        {
            throw new Web3Exception($"{nameof(ChainSafe)} SDK doesn't support chain switching during a single session.");
        }

        /// <summary>
        /// Callback for GetChain.
        /// </summary>
        /// <param name="chainId">New chain.</param>
        public void ChainSelected(string chainId)
        {
            logger.Log($"Selected Chain Id {new HexBigInteger(chainId).Value}.");
        }

        // Callback for displaying an error if operation fails.
        private void DisplayError(string message)
        {
            logger.LogError(message);
        }
    }
}