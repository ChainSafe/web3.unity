using System;
using System.Threading.Tasks;
using Prefabs.Web3AuthWallet.Interfaces;
using UnityEngine;
using Web3Unity.Scripts.Library.ETHEREUEM.Connect;
using Web3Unity.Scripts.Library.Ethers.Transactions;
using Web3Unity.Scripts.Library.Ethers.Web3AuthWallet;

namespace Prefabs.Web3AuthWallet.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionConfig transactionConfig;
        private readonly IHttpRequestHandler httpRequestHandler;
        private readonly IEthereumService ethereumService;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionService"/> class.
        /// </summary>
        /// <param name="transactionConfig">The transaction configuration.</param>
        /// <param name="httpRequestHandler">The HTTP request handler.</param>
        /// <param name="ethereumService">The Ethereum service.</param>
        public TransactionService(ITransactionConfig transactionConfig, IHttpRequestHandler httpRequestHandler, IEthereumService ethereumService)
        {
            this.transactionConfig = transactionConfig;
            this.httpRequestHandler = httpRequestHandler;
            this.ethereumService = ethereumService;
        }

        /// <summary>
        /// Creates a transaction.
        /// </summary>
        /// <param name="txRequest">The transaction request.</param>
        /// <param name="account">The account.</param>
        /// <param name="gasPrice">The gas price.</param>
        /// <param name="gasLimit">The gas limit.</param>
        /// <param name="nonce">The nonce.</param>
        /// <returns>The created transaction.</returns>
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
            string url = W3AWalletUtils.Host + "/createTransaction";
            return await httpRequestHandler.PostRequest<EVM.Response<string>>(url, form);
        }

        /// <summary>
        /// Sends a transaction.
        /// </summary>
        /// <param name="txRequest">The transaction request.</param>
        /// <param name="signature">The transaction signature.</param>
        /// <returns>The transaction response.</returns>
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

        /// <summary>
        /// Broadcasts a transaction.
        /// </summary>
        /// <param name="txRequest">The transaction request.</param>
        /// <param name="account">The account.</param>
        /// <param name="signature">The transaction signature.</param>
        /// <param name="gasPrice">The gas price.</param>
        /// <param name="gasLimit">The gas limit.</param>
        /// <returns>The broadcasted transaction.</returns>
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
            string url = W3AWalletUtils.Host + "/broadcastTransaction";
            return await httpRequestHandler.PostRequest<EVM.Response<string>>(url, form);
        }
    }
}