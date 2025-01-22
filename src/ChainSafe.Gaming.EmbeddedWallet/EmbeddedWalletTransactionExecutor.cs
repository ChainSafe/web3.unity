using System;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Providers;
using ChainSafe.Gaming.Evm.Transactions;
using ChainSafe.Gaming.InProcessSigner;
using ChainSafe.Gaming.Web3.Core.Evm;
using TransactionExecutor = ChainSafe.Gaming.InProcessTransactionExecutor.InProcessTransactionExecutor;

namespace ChainSafe.Gaming.EmbeddedWallet
{
    /// <summary>
    /// Implementation of <see cref="ITransactionExecutor"/> for handling embedded wallet transactions.
    /// </summary>
    public class EmbeddedWalletTransactionExecutor : TransactionExecutor
    {
        private readonly IEmbeddedWalletConfig config;

        private readonly EmbeddedWalletRequestHandler requestHandler;

        public EmbeddedWalletTransactionExecutor(IEmbeddedWalletConfig config, EmbeddedWalletRequestHandler requestHandler, IAccountProvider accountProvider, IRpcProvider rpcProvider)
            : base(accountProvider, rpcProvider)
        {
            this.config = config;

            this.requestHandler = requestHandler;

            requestHandler.RequestApproved += request =>
            {
                if (request is EmbeddedWalletTransaction transaction)
                {
                    TransactionApproved(transaction);
                }
            };

            requestHandler.RequestDeclined += request =>
            {
                if (request is EmbeddedWalletTransaction transaction)
                {
                    TransactionDeclined(transaction);
                }
            };
        }

        public override Task<TransactionResponse> SendTransaction(TransactionRequest request)
        {
            if (config.AutoApproveTransactions)
            {
                return base.SendTransaction(request);
            }

            var transaction = new EmbeddedWalletTransaction(request);

            requestHandler.Enqueue(transaction);

            return transaction.Response.Task;
        }

        private async void TransactionApproved(EmbeddedWalletTransaction transaction)
        {
            try
            {
                var response = await base.SendTransaction(transaction.Request);

                transaction.Response.SetResult(response);
            }
            catch (Exception e)
            {
                transaction.Response.SetException(e);
            }

            requestHandler.Confirm(transaction);
        }

        private void TransactionDeclined(EmbeddedWalletTransaction transaction)
        {
            transaction.Response.SetCanceled();
        }
    }
}