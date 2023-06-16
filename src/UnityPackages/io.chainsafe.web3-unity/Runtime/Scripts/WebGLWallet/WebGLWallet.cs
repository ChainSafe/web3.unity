﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using ChainSafe.GamingSDK.EVM.Web3.Core;
using ChainSafe.GamingSDK.EVM.Web3.Core.Evm;
using ChainSafe.GamingWeb3;
using JetBrains.Annotations;
using Nethereum.ABI.EIP712;
using UnityEngine;
using Web3Unity.Scripts.Library.Ethers.Providers;
using Web3Unity.Scripts.Library.Ethers.Signers;
using Web3Unity.Scripts.Library.Ethers.Transactions;

namespace ChainSafe.GamingSDK.EVM.WebGLWallet
{
    // todo: check if window.web3gl is bound during initialization
    public class WebGLWallet : ISigner, ITransactionExecutor, ILifecycleParticipant
    {
        private readonly IRpcProvider provider;
        [CanBeNull] private string address;

        public WebGLWallet(IRpcProvider provider)
        {
            this.provider = provider;
        }

        public async ValueTask WillStartAsync()
        {
            AssertRunningInWebGLPlayer();

            // Get user address
            JS_resetConnectAccount();
            JS_web3Connect();
            address = await PollJsSide(JS_getConnectAccount);

            void AssertRunningInWebGLPlayer()
            {
                if (Application.platform == RuntimePlatform.WebGLPlayer) return;
                throw new Web3Exception($"{nameof(WebGLWallet)} can only be used on {RuntimePlatform.WebGLPlayer} platform");
            }
        }

        public ValueTask WillStopAsync() => new(Task.CompletedTask);

        public Task<string> GetAddress()
        {
            return Task.FromResult(address);
        }

        public async Task<string> SignMessage(string message)
        {
            JS_resetSignMessageResponse();
            JS_signMessage(message);
            var signedResponse = await PollJsSide(JS_getSignMessageResponse);
            AssertResponseSuccessful(signedResponse);
            return signedResponse;

            void AssertResponseSuccessful(string response)
            {
                // todo: check with regex mb?
                if (response.Length == 132) return;
                throw new Web3Exception("Sign message operation was rejected.");
            }
        }

        public async Task<TransactionResponse> SendTransaction(TransactionRequest transaction)
        {
            var hash = string.IsNullOrEmpty(transaction.Data)
                ? await SendRegularTransaction()
                : await SendTransactionWithData();

            AssertResponseSuccessful(hash);
            var transactionResponse = await provider.GetTransaction(hash);
            return transactionResponse;

            async Task<string> SendRegularTransaction()
            {
                JS_resetSendTransactionResponse();
                JS_sendTransaction(transaction.To, transaction.Value.ToString(), transaction.GasLimit.ToString(),
                    transaction.GasPrice.ToString());
                return await PollJsSide(JS_getSendTransactionResponse);
            }

            async Task<string> SendTransactionWithData()
            {
                JS_resetSendTransactionResponseData();
                JS_sendTransactionData(transaction.To, transaction.Value.ToString(), transaction.GasLimit.ToString(),
                    transaction.GasPrice.ToString(), transaction.Data);
                return await PollJsSide(JS_getSendTransactionResponseData);
            }

            void AssertResponseSuccessful(string response)
            {
                // todo: check with regex mb?
                if (response.Length == 66) return;
                throw new Web3Exception("Send transaction operation was rejected.");
            }
        }

        // todo: implement before release
        public Task<string> SignTypedData(Domain domain, Dictionary<string, MemberDescription[]> types, MemberValue[] message)
        {
            throw new NotImplementedException();
        }

        // todo: will break if running two of the same Poll tasks from different signers
        private static async Task<string> PollJsSide(Func<string> getMethod)
        {
            string jsResponse;
            do
            {
                jsResponse = getMethod();
                await Task.Yield();
            } while (string.IsNullOrEmpty(jsResponse));
            return jsResponse;
        }

        // Connect
        [DllImport("__Internal")]
        private static extern void JS_web3Connect();
        [DllImport("__Internal")]
        private static extern string JS_getConnectAccount();
        [DllImport("__Internal")]
        private static extern void JS_resetConnectAccount();

        // SignMessage
        [DllImport("__Internal")]
        private static extern void JS_signMessage(string value);
        [DllImport("__Internal")]
        private static extern string JS_getSignMessageResponse();
        [DllImport("__Internal")]
        private static extern void JS_resetSignMessageResponse();

        // SendTransaction (no data)
        [DllImport("__Internal")]
        private static extern void JS_sendTransaction(string to, string value, string gasLimit, string gasPrice);
        [DllImport("__Internal")]
        private static extern string JS_getSendTransactionResponse();
        [DllImport("__Internal")]
        private static extern void JS_resetSendTransactionResponse();

        // SendTransaction (with data)
        [DllImport("__Internal")]
        private static extern void JS_sendTransactionData(string to, string value, string gasPrice, string gasLimit,
            string data);
        [DllImport("__Internal")]
        private static extern string JS_getSendTransactionResponseData();
        [DllImport("__Internal")]
        private static extern void JS_resetSendTransactionResponseData();
    }
}