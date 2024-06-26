using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm;
using ChainSafe.Gaming.Unity.EthereumWindow.Dto;
using ChainSafe.Gaming.Web3;
using Nethereum.Hex.HexTypes;
using Nethereum.JsonRpc.Client.RpcMessages;
using Newtonsoft.Json;
using UnityEngine;

namespace ChainSafe.Gaming.Unity.EthereumWindow
{
    /// <summary>
    /// A controller script for making awaitable JsonRPC requests to browser wallets via window.ethereum.
    /// Make sure EthereumWindow.jslib is present in your Web3.Unity Package.
    /// https://github.com/ChainSafe/web3.unity/blob/main/Packages/io.chainsafe.web3-unity/Runtime/Plugins/EthereumWindow/EthereumWindow.jslib.
    /// </summary>
    public class EthereumWindowController : MonoBehaviour
    {
        private readonly Dictionary<string, TaskCompletionSource<RpcResponseMessage>> requestTcsMap = new Dictionary<string, TaskCompletionSource<RpcResponseMessage>>();

        private TaskCompletionSource<string> connectionTcs;

        [DllImport("__Internal")]
        public static extern string Request(string message, string gameObjectName, string callback, string fallback);

        [DllImport("__Internal")]
        public static extern string Connect(string chain, string gameObjectName, string callback, string fallback);

        /// <summary>
        /// Make JsonRPC request to the HyperPlay side-loaded browser games on HyperPlay desktop client.
        /// </summary>
        /// <param name="method">JsonRPC method name.</param>
        /// <param name="parameters">JsonRPC request parameters.</param>
        /// <returns>Rpc Response.</returns>
        public virtual Task<RpcResponseMessage> Request(string method, params object[] parameters)
        {
            string id = Guid.NewGuid().ToString();

            var request = new RpcRequestMessage(id, method, parameters);

            string message = JsonConvert.SerializeObject(request);

            var requestTcs = new TaskCompletionSource<RpcResponseMessage>();

            requestTcsMap.Add(id, requestTcs);

            Request(message, gameObject.name, nameof(Response), nameof(RequestError));

            return requestTcs.Task;
        }

        /// <summary>
        /// JsonRPC Response callback.
        /// </summary>
        /// <param name="result">Response Result string.</param>
        /// <exception cref="Web3Exception">Throws Exception if response has errors.</exception>
        public void Response(string result)
        {
            var response = JsonConvert.DeserializeObject<RpcResponseMessage>(result);

            if (requestTcsMap.TryGetValue(response.Id.ToString(), out TaskCompletionSource<RpcResponseMessage> requestTcs))
            {
                if (!requestTcs.TrySetResult(response))
                {
                    requestTcs.SetException(new Web3Exception("Error setting result."));
                }
            }
            else
            {
                throw new Web3Exception("Can't find Request Task.");
            }
        }

        /// <summary>
        /// Request Error callback.
        /// </summary>
        /// <param name="error">Error message.</param>
        /// <exception cref="Web3Exception">Throws exception if setting error fails.</exception>
        public void RequestError(string error)
        {
            var response = JsonConvert.DeserializeObject<RpcResponseMessage>(error);

            if (requestTcsMap.TryGetValue(response.Id.ToString(), out TaskCompletionSource<RpcResponseMessage> requestTcs))
            {
                if (!requestTcs.TrySetException(new Web3Exception(response.Error.Message)))
                {
                    requestTcs.SetException(new Web3Exception($"Error setting error: {response.Error.Message}"));
                }
            }
            else
            {
                throw new Web3Exception("Can't find request Task.");
            }
        }

        /// <summary>
        /// Connects to an Ethereum window (browser) wallet.
        /// </summary>
        /// <param name="chainConfig">Chain config for what chain to connect to.</param>
        /// <param name="chainRegistryProvider">List of all known chains with details.</param>
        /// <returns>Connected account address.</returns>
        /// <exception cref="Web3Exception">Throws Exception if connection is unsuccessful.</exception>
        public virtual async Task<string> Connect(IChainConfig chainConfig, ChainRegistryProvider chainRegistryProvider)
        {
            var chain = await WalletChain(chainConfig, chainRegistryProvider);

            if (connectionTcs != null && !connectionTcs.Task.IsCompleted)
            {
                connectionTcs.SetCanceled();
            }

            connectionTcs = new TaskCompletionSource<string>();

            Connect(JsonConvert.SerializeObject(chain), gameObject.name, nameof(Connected), nameof(ConnectError));

            return await connectionTcs.Task;
        }

        private async Task<WalletChain> WalletChain(IChainConfig chainConfig, ChainRegistryProvider chainRegistryProvider)
        {
            int chainId = int.Parse(chainConfig.ChainId);

            var registryChain = await chainRegistryProvider.GetChain((ulong)chainId);

            var nativeCurrency = registryChain?.NativeCurrencyInfo;

            string hexChainId = new BigInteger(chainId).ToHexBigInteger().HexValue;

            return new WalletChain
            {
                ChainId = hexChainId,
                Name = registryChain != null ? registryChain.Name : chainConfig.Chain,
                RpcUrls = registryChain != null ? registryChain.RPC : new string[] { chainConfig.Rpc },
                NativeCurrency = registryChain != null
                    ? new NativeCurrency
                    {
                        Name = nativeCurrency.Name,
                        Symbol = nativeCurrency.Symbol,
                        Decimals = (int)nativeCurrency.Decimals,
                    }
                    : new NativeCurrency(chainConfig.Symbol),
            };
        }

        public void Connected(string result)
        {
            connectionTcs.SetResult(result);
        }

        public void ConnectError(string error)
        {
            connectionTcs.SetException(new Web3Exception(error));
        }
    }
}