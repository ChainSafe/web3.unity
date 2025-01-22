using System;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Transactions;
using Nethereum.Util;

namespace ChainSafe.Gaming.EmbeddedWallet
{
    /// <summary>
    /// Transaction object for handling embedded wallet transactions.
    /// </summary>
    public class EmbeddedWalletTransaction : IEmbeddedWalletRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmbeddedWalletTransaction"/> class.
        /// </summary>
        /// <param name="request">Initial transaction request.</param>
        /// <param name="symbol">Native Symbol of the chain.</param>
        /// <param name="blockExplorerUrl">Base block explorer url.</param>
        public EmbeddedWalletTransaction(TransactionRequest request, string symbol, string blockExplorerUrl)
        {
            Request = request;

            Response = new TaskCompletionSource<TransactionResponse>();

            if (Request.Value != null)
            {
                ValueString = $"{UnitConversion.Convert.FromWei(Request.Value)} {symbol}";
            }

            BlockExplorerUrl = blockExplorerUrl;
        }

        public DateTime Timestamp { get; private set; }

        public string BlockExplorerUrl { get; private set; }

        /// <summary>
        /// Initial Transaction Request.
        /// </summary>
        public TransactionRequest Request { get; private set; }

        /// <summary>
        /// Awaitable Transaction Response.
        /// </summary>
        public TaskCompletionSource<TransactionResponse> Response { get; private set; }

        public string ValueString { get; private set; }

        public void Confirm()
        {
            Timestamp = DateTime.Now;

            if (!string.IsNullOrEmpty(BlockExplorerUrl))
            {
                if (!BlockExplorerUrl.EndsWith('/'))
                {
                    BlockExplorerUrl += '/';
                }

                BlockExplorerUrl += $"tx/{Response.Task.Result.Hash}";
            }
        }
    }
}