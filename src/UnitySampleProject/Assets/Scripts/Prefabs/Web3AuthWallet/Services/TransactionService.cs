using System;
using System.Threading.Tasks;
using ChainSafe.GamingSDK.EVM.Web3AuthWallet;
using Prefabs.Web3AuthWallet.Interfaces;
using UnityEngine;
using Web3Unity.Scripts.Library.ETHEREUEM.Connect;
using Web3Unity.Scripts.Library.Ethers.Transactions;

namespace Prefabs.Web3AuthWallet.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionConfig transactionConfig;
        private readonly IHttpRequestHandler httpRequestHandler;
        private readonly IEthereumService ethereumService;

        public TransactionService(ITransactionConfig transactionConfig, IHttpRequestHandler httpRequestHandler, IEthereumService ethereumService)
        {
            this.transactionConfig = transactionConfig;
            this.httpRequestHandler = httpRequestHandler;
            this.ethereumService = ethereumService;
        }

        public async Task<EVM.Response<string>> CreateTransaction(TransactionRequest txRequest, string account, string gasPrice, string gasLimit, string nonce)
        {
            WWWForm form = new WWWForm();
            form.AddField("projectId", transactionConfig.GetProjectId());
            form.AddField("chain", transactionConfig.GetChain());
            form.AddField("network", transactionConfig.GetNetwork());
            form.AddField("account", account);
            form.AddField("to", txRequest.To);
            form.AddField("value", txRequest.Value.ToString());
            form.AddField("data", txRequest.Data);
            form.AddField("gasPrice", gasPrice);
            form.AddField("gasLimit", gasLimit);
            form.AddField("rpc", transactionConfig.GetRpc());
            form.AddField("nonce", nonce);
            string url = "https://api.gaming.chainsafe.io/evm/createTransaction";
            return await httpRequestHandler.PostRequest<EVM.Response<string>>(url, form);
        }

        public Task<TransactionResponse> SendTransaction(TransactionRequest txRequest, string signature)
        {
            /*WWWForm form = new WWWForm();
            form.AddField("projectId", PlayerPrefs.GetString("ProjectID"));
            form.AddField("chain", transactionConfig.GetChain());
            form.AddField("network", transactionConfig.GetNetwork());
            form.AddField("account", txRequest.From);
            form.AddField("to", txRequest.To);
            form.AddField("value", txRequest.Value.ToString());
            form.AddField("data", txRequest.Data);
            form.AddField("signature", W3AUtils.SignedTxResponse);
            form.AddField("gasPrice", txRequest.GasPrice.ToString());
            form.AddField("gasLimit", txRequest.GasLimit.ToString());
            form.AddField("rpc", transactionConfig.GetRpc());
            string url = "https://api.gaming.chainsafe.io/evm/broadcastTransaction";
            var result = httpRequestHandler.PostRequest<Web3AuthWallet.ResponseObject<TransactionResponse>>(url, form)?.Result;
            return result;*/
            throw new NotImplementedException();
        }

        public async Task<EVM.Response<string>> BroadcastTransaction(TransactionRequest txRequest, string account, string signature, string gasPrice, string gasLimit)
        {
            WWWForm form = new WWWForm();
            form.AddField("projectId", transactionConfig.GetProjectId());
            form.AddField("chain", transactionConfig.GetChain());
            form.AddField("network", transactionConfig.GetNetwork());
            form.AddField("account", account);
            form.AddField("to", txRequest.To);
            form.AddField("value", txRequest.Value.HexValue);
            form.AddField("data", txRequest.Data);
            form.AddField("signature", signature);
            form.AddField("gasPrice", gasPrice);
            form.AddField("gasLimit", gasLimit);
            form.AddField("rpc", transactionConfig.GetRpc());
            string url = "https://api.gaming.chainsafe.io/evm/broadcastTransaction";
            return await httpRequestHandler.PostRequest<EVM.Response<string>>(url, form);
        }
    }
}