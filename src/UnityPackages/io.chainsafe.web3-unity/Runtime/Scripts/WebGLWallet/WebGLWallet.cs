using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using ChainSafe.GamingSDK.EVM.Web3.Core.Evm;
using ChainSafe.GamingWeb3;
using Nethereum.ABI.EIP712;
using Web3Unity.Scripts.Library.Ethers.Providers;
using Web3Unity.Scripts.Library.Ethers.Signers;
using Web3Unity.Scripts.Library.Ethers.Transactions;

namespace ChainSafe.GamingSDK.EVM.WebGLWallet
{
    // todo: check if window.web3gl is bound during initialization
    public class WebGLWallet : ISigner, ITransactionExecutor
    {
        private readonly IRpcProvider provider;

        public WebGLWallet(IRpcProvider provider)
        {
            this.provider = provider;
        }

        public Task<string> GetAddress()
        {
            throw new NotImplementedException();
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
                if (response.Length != 132)
                {
                    throw new Web3Exception("Sign message operation was rejected.");
                }
            }
        }

        // todo: implement
        public Task<string> SignTransaction(TransactionRequest transaction)
        {
            throw new NotImplementedException();
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
                if (response.Length != 66)
                {
                    throw new Web3Exception("Send transaction operation was rejected.");
                }
            }
        }

        public Task<string> SignTypedData(Domain domain, Dictionary<string, MemberDescription[]> types, MemberValue[] message)
        {
            throw new NotImplementedException();
        }

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