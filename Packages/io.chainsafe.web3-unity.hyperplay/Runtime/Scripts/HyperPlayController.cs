using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using ChainSafe.Gaming.HyperPlay.Dto;
using ChainSafe.Gaming.Web3;
using Nethereum.JsonRpc.Client.RpcMessages;
using Newtonsoft.Json;
using UnityEngine;

namespace ChainSafe.Gaming.HyperPlay
{
    /// <summary>
    /// A controller script for side-loaded browser games and making awaitable JsonRPC requests.
    /// </summary>
    public class HyperPlayController : MonoBehaviour
    {
        [DllImport("__Internal")]
        public static extern string Request(string message, string gameObjectName, string callback, string fallback);
        
        [DllImport("__Internal")]
        public static extern string Connect(string chain, string gameObjectName, string callback, string fallback);

        private readonly Dictionary<string, TaskCompletionSource<RpcResponseMessage>> _requestTcsMap = new Dictionary<string, TaskCompletionSource<RpcResponseMessage>>();

        private TaskCompletionSource<string> _connectionTcs;

        /// <summary>
        /// Make JsonRPC request to the HyperPlay side-loaded browser games on HyperPlay desktop client.
        /// </summary>
        /// <param name="method">JsonRPC method name.</param>
        /// <param name="parameters">JsonRPC request parameters.</param>
        /// <returns>Rpc Response.</returns>
        public Task<RpcResponseMessage> Request(string method, params object[] parameters)
        {
            string id = Guid.NewGuid().ToString();
            
            var request = new RpcRequestMessage(id, method, parameters);

            string message = JsonConvert.SerializeObject(request);
            
            var requestTcs = new TaskCompletionSource<RpcResponseMessage>();
            
            _requestTcsMap.Add(id, requestTcs);
            
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

            if (_requestTcsMap.TryGetValue(response.Id.ToString(), out TaskCompletionSource<RpcResponseMessage> requestTcs))
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
            
            if (_requestTcsMap.TryGetValue(response.Id.ToString(), out TaskCompletionSource<RpcResponseMessage> requestTcs))
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

        public Task<string> Connect(Chain chain)
        {
            if (_connectionTcs != null && !_connectionTcs.Task.IsCompleted)
            {
                _connectionTcs.SetCanceled();
            }

            _connectionTcs = new TaskCompletionSource<string>();
            
            Connect(JsonConvert.SerializeObject(chain), gameObject.name, nameof(Connected), nameof(ConnectError));

            return _connectionTcs.Task;
        }
        
        public void Connected(string result)
        {
            _connectionTcs.SetResult(result);
        }
        
        public void ConnectError(string error)
        {
            _connectionTcs.SetException(new Web3Exception(error));
        }
    }
}
