using System;
using System.Numerics;
using System.Net.WebSockets;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Transactions;
using ChainSafe.Gaming.Evm.Contracts;
using ChainSafe.Gaming.RPC.Events;
using Nethereum.Hex.HexTypes;
using Nethereum.Contracts;
using Nethereum.RPC.Reactive.Eth.Subscriptions;
using Nethereum.JsonRpc.WebSocketStreamingClient;
using Nethereum.ABI.FunctionEncoding.Attributes;
using UnityEngine;


namespace ChainSafe.Gaming.Evm.Contracts.Custom
{
    public class EchoChainContract : ICustomContract
    {
        public string Address => OriginalContract.Address;

        public string ABI =>
            "[     {         \"inputs\": [],         \"name\": \"echoChain\",         \"outputs\": [             {                 \"internalType\": \"string\",                 \"name\": \"\",                 \"type\": \"string\"             }         ],         \"stateMutability\": \"pure\",         \"type\": \"function\"     } ]";

        public string ContractAddress { get; set; }

        public IContractBuilder ContractBuilder { get; set; }

        public Contract OriginalContract { get; set; }
        public IEventManager EventManager { get; set; }

        public string WebSocketUrl { get; set; }

        public bool Subscribed { get; set; }

        private StreamingWebSocketClient _webSocketClient;

        #region Methods

        public async Task<string> EchoChain()
        {
            var response = await OriginalContract.Call<string>("echoChain", new object[]
            {
            });

            return response;
        }

        #endregion


        #region Interface Implemented Methods

        public async ValueTask DisposeAsync()
        {
            if (string.IsNullOrEmpty(WebSocketUrl))
                return;
            if (!Subscribed)
                return;

            if (Application.platform == RuntimePlatform.WebGLPlayer)
                return;
            Subscribed = false;
            try
            {
                if (_webSocketClient != null)
                    await _webSocketClient.StopAsync();
            }
            catch (Exception e)
            {
                Debug.LogError("Caught an exception whilst unsubscribing from events\n" + e.Message);
            }
        }

        public async ValueTask InitAsync()
        {
            if (Subscribed)
                return;
            Subscribed = true;

            if (string.IsNullOrEmpty(WebSocketUrl))
            {
                Debug.LogWarning($"WebSocketUrl is not set for this class. Event Subscriptions will not work.");
                return;
            }


            if ((_webSocketClient != null && _webSocketClient.WebSocketState != WebSocketState.Open) ||
                _webSocketClient.WebSocketState != WebSocketState.CloseReceived)
            {
                Debug.LogWarning(
                    $"Websocket is in an invalid state {_webSocketClient.WebSocketState}. It needs to be in a state Open or CloseReceived");
                return;
            }

            try
            {
                if (Application.platform == RuntimePlatform.WebGLPlayer)
                {
                    Debug.LogWarning("WebGL Platform is currently not supporting event subscription");
                    return;
                }

                _webSocketClient ??= new StreamingWebSocketClient(WebSocketUrl);
                await _webSocketClient.StartAsync();
            }
            catch (Exception e)
            {
                Debug.LogError(
                    "Caught an exception whilst subscribing to events. Subscribing to events will not work in this session\n" +
                    e.Message);
            }
        }

        [Obsolete("It's not advisable to use this method. Use the pre-generated methods instead.")]
        public IContract Attach(string address)
        {
            return OriginalContract.Attach(address);
        }

        [Obsolete("It's not advisable to use this method. Use the pre-generated methods instead.")]
        public Task<object[]> Call(string method, object[] parameters = null, TransactionRequest overwrite = null)
        {
            return OriginalContract.Call(method, parameters, overwrite);
        }

        [Obsolete("It's not advisable to use this method. Use the pre-generated methods instead.")]
        public object[] Decode(string method, string output)
        {
            return OriginalContract.Decode(method, output);
        }

        [Obsolete("It's not advisable to use this method. Use the pre-generated methods instead.")]
        public Task<object[]> Send(string method, object[] parameters = null, TransactionRequest overwrite = null)
        {
            return OriginalContract.Send(method, parameters, overwrite);
        }

        [Obsolete("It's not advisable to use this method. Use the pre-generated methods instead.")]
        public Task<(object[] response, TransactionReceipt receipt)> SendWithReceipt(string method,
            object[] parameters = null, TransactionRequest overwrite = null)
        {
            return OriginalContract.SendWithReceipt(method, parameters, overwrite);
        }

        [Obsolete("It's not advisable to use this method. Use the pre-generated methods instead.")]
        public Task<HexBigInteger> EstimateGas(string method, object[] parameters, TransactionRequest overwrite = null)
        {
            return OriginalContract.EstimateGas(method, parameters, overwrite);
        }

        [Obsolete("It's not advisable to use this method. Use the pre-generated methods instead.")]
        public string Calldata(string method, object[] parameters = null)
        {
            return OriginalContract.Calldata(method, parameters);
        }

        [Obsolete("It's not advisable to use this method. Use the pre-generated methods instead.")]
        public Task<TransactionRequest> PrepareTransactionRequest(string method, object[] parameters,
            bool isReadCall = false, TransactionRequest overwrite = null)
        {
            return OriginalContract.PrepareTransactionRequest(method, parameters, isReadCall, overwrite);
        }

        #endregion
    }
}