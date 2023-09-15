using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Providers;
using ChainSafe.Gaming.Evm.Signers;
using ChainSafe.Gaming.Evm.Transactions;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Core;
using ChainSafe.Gaming.Web3.Core.Debug;
using ChainSafe.Gaming.Web3.Core.Evm;
using JetBrains.Annotations;
using Nethereum.ABI.EIP712;
using Newtonsoft.Json;
using UnityEngine;

namespace ChainSafe.Gaming.Wallets
{
#if !UNITY_EDITOR && UNITY_WEBGL
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
            // Get user address
            JS_resetConnectAccount();
            JS_web3Connect();
            address = await PollJsSide(JS_getConnectAccount);
        }

        public ValueTask WillStopAsync()
        {
            return new ValueTask(Task.CompletedTask);
        }

        public Task<string> GetAddress()
        {
            address.AssertNotNull(nameof(address));
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
                JS_sendTransaction(
                    transaction.To,
                    transaction.Value?.ToString() ?? "",
                    transaction.GasLimit?.ToString() ?? "",
                    transaction.GasPrice?.ToString() ?? "");
                return await PollJsSide(JS_getSendTransactionResponse);
            }

            async Task<string> SendTransactionWithData()
            {
                JS_resetSendTransactionResponseData();
                JS_sendTransactionData(
                    transaction.To,
                    transaction.Value?.ToString() ?? "",
                    transaction.GasLimit?.ToString() ?? "",
                    transaction.GasPrice?.ToString() ?? "",
                    transaction.Data);
                return await PollJsSide(JS_getSendTransactionResponseData);
            }

            void AssertResponseSuccessful(string response)
            {
                // todo: check with regex mb?
                if (response.Length == 66) return;
                throw new Web3Exception("Send transaction operation was rejected.");
            }
        }

        public async Task<string> SignTypedData<TStructType>(SerializableDomain domain, TStructType message)
        {
            var types = MemberDescriptionFactory.GetTypesMemberDescription(typeof(TStructType));
            JS_resetSignTypedMessageResponse();
            JS_signTypedMessage(JsonConvert.SerializeObject(domain), JsonConvert.SerializeObject(types),
                JsonConvert.SerializeObject(message));
            var signedTypedMessageResponse = await PollJsSide(JS_getSignTypedMessageResponse);
            AssertResponseSuccessful(signedTypedMessageResponse);
            return signedTypedMessageResponse;

            void AssertResponseSuccessful(string response)
            {
                // todo: check with regex mb?
                if (response.Length == 132) return;
                throw new Web3Exception("Sign message operation was rejected.");
            }
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

        // SignTypedMessage
        [DllImport("__Internal")]
        private static extern void JS_signTypedMessage(string domain, string types, string message);

        [DllImport("__Internal")]
        private static extern string JS_getSignTypedMessageResponse();

        [DllImport("__Internal")]
        private static extern void JS_resetSignTypedMessageResponse();

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
#else
    // Stub implementation for other platforms
    public class WebGLWallet : ISigner, ITransactionExecutor, ILifecycleParticipant
    {
        public Task<string> GetAddress()
        {
            throw new NotImplementedException();
        }

        public Task<TransactionResponse> SendTransaction(TransactionRequest transaction)
        {
            throw new NotImplementedException();
        }

        public Task<string> SignMessage(string message)
        {
            throw new NotImplementedException();
        }

        public Task<string> SignTypedData<TStructType>(SerializableDomain domain, TStructType message)
        {
            throw new NotImplementedException();
        }

        public ValueTask WillStartAsync()
        {
            throw new Web3Exception(
                $"{nameof(WebGLWallet)} can only be used on {RuntimePlatform.WebGLPlayer} platform");
        }

        public ValueTask WillStopAsync()
        {
            throw new NotImplementedException();
        }
    }
#endif
}