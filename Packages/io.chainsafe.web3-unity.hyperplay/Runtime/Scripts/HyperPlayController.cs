using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using ChainSafe.Gaming.Web3;
using Nethereum.JsonRpc.Client.RpcMessages;
using Newtonsoft.Json;
using UnityEngine;

namespace ChainSafe.Gaming.HyperPlay
{
    public class HyperPlayController : MonoBehaviour
    {
        [DllImport("__Internal")]
        public static extern string Request(string message, string gameObjectName, string callback, string fallback);

        private readonly Dictionary<string, TaskCompletionSource<RpcResponseMessage>> _requestTcsMap = new Dictionary<string, TaskCompletionSource<RpcResponseMessage>>();
        
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

        public void Response(string result)
        {
            var response = JsonConvert.DeserializeObject<RpcResponseMessage>(result);

            if (_requestTcsMap.TryGetValue(response.Id.ToString(), out TaskCompletionSource<RpcResponseMessage> requestTcs))
            {
                if (!requestTcs.TrySetResult(response))
                {
                    requestTcs.SetException(new Web3Exception(""));
                }
            }

            else
            {
                throw new Web3Exception("");
            }
        }
        
        public void RequestError(string error)
        {
            var response = JsonConvert.DeserializeObject<RpcResponseMessage>(error);
            
            if (_requestTcsMap.TryGetValue(response.Id.ToString(), out TaskCompletionSource<RpcResponseMessage> requestTcs))
            {
                if (!requestTcs.TrySetException(new Web3Exception(response.Error.Message)))
                {
                    requestTcs.SetException(new Web3Exception(""));
                }
            }

            else
            {
                throw new Web3Exception("");
            }
        }
    }
}
