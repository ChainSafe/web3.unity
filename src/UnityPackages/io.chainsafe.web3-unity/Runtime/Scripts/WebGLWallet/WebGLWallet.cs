using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using ChainSafe.GamingSDK.EVM.Web3.Core.Evm;
using Nethereum.ABI.EIP712;
using Web3Unity.Scripts.Library.Ethers.Signers;
using Web3Unity.Scripts.Library.Ethers.Transactions;

namespace ChainSafe.GamingSDK.EVM.WebGLWallet
{
    // todo: check if window.web3gl is bound
    public class WebGLWallet : ISigner, ITransactionExecutor
    {
        public Task<string> GetAddress()
        {
            throw new NotImplementedException();
        }

        public async Task<string> SignMessage(string message)
        {
            JS_signMessage(message);
            JS_resetSignMessageResponse();
            var signedResponse = await PollJsSide(JS_getSignMessageResponse);
            return signedResponse;
        }

        // todo: serialize transaction then sign??
        public Task<string> SignTransaction(TransactionRequest transaction)
        {
            throw new NotImplementedException();
        }

        public Task<TransactionResponse> SendTransaction(TransactionRequest transaction)
        {
            throw new NotImplementedException();

            return string.IsNullOrEmpty(transaction.Data)
                ? SendRegularTransaction()
                : SendTransactionWithData();

            // return await PollJsSide(JS_getSignMessageResponse);

            async Task<TransactionResponse> SendRegularTransaction()
            {
                throw new NotImplementedException();
                //JS_sendTransaction(transaction.To, transaction.Value.ToString(), transaction.GasLimit, transaction.GasPrice);
            }

            async Task<TransactionResponse> SendTransactionWithData()
            {
                throw new NotImplementedException();
            }
        }

        public Task<string> SignTypedData(Domain domain, Dictionary<string, MemberDescription[]> types, MemberValue[] message)
        {
            throw new NotImplementedException();
        }
            
        private static async Task<string> PollJsSide(Func<string> pollMethod)
        {
            string jsResponse;
            do
            {
                jsResponse = pollMethod();
                await Task.Yield();
            } while (string.IsNullOrEmpty(jsResponse));
            return jsResponse;
        }
        
        [DllImport("__Internal")]
        private static extern void JS_signMessage(string value);
        [DllImport("__Internal")]
        private static extern string JS_getSignMessageResponse();
        [DllImport("__Internal")]
        private static extern void JS_resetSignMessageResponse();
        
        [DllImport("__Internal")]
        private static extern void JS_sendTransaction(string to, string value, string gasLimit, string gasPrice);
        [DllImport("__Internal")]
        private static extern string JS_getSendTransactionResponse();
        [DllImport("__Internal")]
        private static extern void JS_resetSendTransactionResponse();
        
        [DllImport("__Internal")]
        private static extern void JS_sendTransactionData(string to, string value, string gasPrice, string gasLimit,
            string data);
        [DllImport("__Internal")]
        private static extern string JS_getSendTransactionResponseData();
        [DllImport("__Internal")]
        private static extern void JS_resetSendTransactionResponseData();
    }
}