using System;
using System.Collections;
using System.Threading.Tasks;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Environment;
using Nethereum.Hex.HexTypes;
using Nethereum.JsonRpc.Client;
using Nethereum.Unity.Metamask;
using Nethereum.Unity.Rpc;
using Newtonsoft.Json;
using UnityEngine;

namespace ChainSafe.Gaming.MetaMask.Unity
{
    public class MetaMaskController : MonoBehaviour
    {
        private bool isInitialized;

        private ILogWriter logger;

        private string connectedAddress;

        public delegate void AccountConnected(string address);

        public delegate void RequestCallback(string requestGuid, string result);

        public event AccountConnected OnAccountConnected;

        public event RequestCallback OnRequestCallback;

        public void Initialize(ILogWriter logWriter)
        {
            logger = logWriter;
        }

        public Task<string> Connect()
        {
            var taskCompletionSource = new TaskCompletionSource<string>();

            OnAccountConnected -= Connected;

            OnAccountConnected += Connected;

            void Connected(string address)
            {
                connectedAddress = address;

                if (!taskCompletionSource.TrySetResult(connectedAddress))
                {
                    logger.LogError("Error setting connected account address.");
                }
                else
                {
                    logger.Log($"MetaMask successfully connected to address {connectedAddress}");
                }
            }

            if (MetamaskWebglInterop.IsMetamaskAvailable())
            {
                MetamaskWebglInterop.EnableEthereum(gameObject.name, nameof(EthereumEnabled), nameof(DisplayError));
            }
            else
            {
                DisplayError("Metamask is not available, please install it.");

                // Unsubscribe to event.
                OnAccountConnected -= Connected;

                return null;
            }

            return taskCompletionSource.Task;
        }

        private IEnumerator Request<T>(string requestGuid, RpcRequest data)
        {
            var rpcRequest = new UnityRpcRequest<T>(GetRpcRequestFactory());

            yield return rpcRequest.SendRequest(data);

            if (rpcRequest.Exception != null)
            {
                logger.LogError($"MetaMask Exception while making {data.Method} request {rpcRequest.Exception.Message}");

                // Even if request fails we still need to callback so request task completion source result can be set.
                InvokeRequestCallback(requestGuid, string.Empty);

                throw rpcRequest.Exception;
            }

            string serializedResult = JsonConvert.SerializeObject(rpcRequest.Result);

            logger.Log($"Successful {data.Method} JsonRPC response with result {serializedResult}");

            InvokeRequestCallback(requestGuid, serializedResult);
        }

        public Task<T> Request<T>(string method, params object[] parameters)
        {
            string requestGuid = Guid.NewGuid().ToString();

            var taskCompletionSource = new TaskCompletionSource<T>();

            OnRequestCallback += RequestCallback;

            void RequestCallback(string guid, string result)
            {
                if (guid != requestGuid)
                {
                    return;
                }

                if (string.IsNullOrEmpty(result))
                {
                    taskCompletionSource.SetException(new Web3Exception("No response received."));

                    return;
                }

                taskCompletionSource.SetResult(JsonConvert.DeserializeObject<T>(result));
            }

            StartCoroutine(Request<T>(requestGuid, new RpcRequest(Configuration.DefaultRequestId, method, parameters)));

            return taskCompletionSource.Task;
        }

        public IUnityRpcRequestClientFactory GetRpcRequestFactory()
        {
            if (MetamaskWebglInterop.IsMetamaskAvailable())
            {
                return new MetamaskWebglCoroutineRequestRpcClientFactory(connectedAddress);
            }
            else
            {
                DisplayError("Metamask is not available, please install it.");
                return null;
            }
        }

        public void EthereumEnabled(string address)
        {
            logger.Log("Ethereum Enabled.");

            if (!isInitialized)
            {
                MetamaskWebglInterop.EthereumInit(gameObject.name, nameof(NewAccountSelected), nameof(ChainChanged));
                MetamaskWebglInterop.GetChainId(gameObject.name, nameof(ChainChanged), nameof(DisplayError));

                isInitialized = true;
            }

            InvokeAccountConnected(address);
        }

        private void InvokeAccountConnected(string address)
        {
            OnAccountConnected?.Invoke(address);
        }

        private void InvokeRequestCallback(string requestGuid, string result)
        {
            OnRequestCallback?.Invoke(requestGuid, result);
        }

        public void NewAccountSelected(string address)
        {
            logger.Log("New Account Selected.");
        }

        public void ChainChanged(string chainId)
        {
            logger.Log($"Selected Chain Id {new HexBigInteger(chainId).Value}.");
        }

        public void DisplayError(string message)
        {
            logger.LogError(message);
        }
    }
}