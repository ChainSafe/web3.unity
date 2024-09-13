﻿using System.Threading.Tasks;
using System;
using ChainSafe.Gaming.Evm.Transactions;
using Nethereum.Hex.HexTypes;
using ChainSafe.Gaming.Evm.Contracts;
using System.Numerics;
using Nethereum.RPC.Reactive.Eth.Subscriptions;
using Nethereum.JsonRpc.WebSocketStreamingClient;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;
using UnityEngine;



namespace ChainSafe.Gaming.Evm.Contracts.Custom
{
    public class EchoChainContract : ICustomContract
    {
        public string Address => OriginalContract.Address;
       
        public string ABI => "[ 	{ 		\"inputs\": [], 		\"name\": \"echoChain\", 		\"outputs\": [ 			{ 				\"internalType\": \"string\", 				\"name\": \"\", 				\"type\": \"string\" 			} 		], 		\"stateMutability\": \"pure\", 		\"type\": \"function\" 	} ]";
        
        public string ContractAddress { get; set; }
        
        public IContractBuilder ContractBuilder { get; set; }

        public Contract OriginalContract { get; set; }
        
        public string WebSocketUrl { get; set; }
        
        public bool Subscribed { get; set; }

        private StreamingWebSocketClient _webSocketClient;
        
        #region Methods

        public async Task<string> EchoChain() 
        {
            var response = await OriginalContract.Call<string>("echoChain", new object [] {
                
            });
            
            return response;
        }



        #endregion
        
        
        #region Event Classes


        #endregion
        
        #region Interface Implemented Methods
        
        public async ValueTask DisposeAsync()
        {
            if(string.IsNullOrEmpty(WebSocketUrl))
                return;
            if(!Subscribed)
                return;
            Subscribed = false;


            _webSocketClient?.Dispose();
        }
        
        public async ValueTask Init()
        {
            if(Subscribed)
                return;
                
            if(string.IsNullOrEmpty(WebSocketUrl))
            {
                Debug.LogWarning($"WebSocketUrl is not set for this class. Event Subscriptions will not work.");
                return;
            }
           
            _webSocketClient ??= new StreamingWebSocketClient(WebSocketUrl);
            await _webSocketClient.StartAsync();
            Subscribed = true;


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
        public Task<(object[] response, TransactionReceipt receipt)> SendWithReceipt(string method, object[] parameters = null, TransactionRequest overwrite = null)
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
        public Task<TransactionRequest> PrepareTransactionRequest(string method, object[] parameters, bool isReadCall = false, TransactionRequest overwrite = null)
        {
            return OriginalContract.PrepareTransactionRequest(method, parameters, isReadCall, overwrite);
        }
        #endregion
    }


}