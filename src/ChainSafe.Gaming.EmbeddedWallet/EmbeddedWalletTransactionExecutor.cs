using System;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Providers;
using ChainSafe.Gaming.Evm.Transactions;
using ChainSafe.Gaming.InProcessSigner;
using TransactionExecutor = ChainSafe.Gaming.InProcessTransactionExecutor.InProcessTransactionExecutor;

namespace ChainSafe.Gaming.EmbeddedWallet
{
    public class EmbeddedWalletTransactionExecutor : TransactionExecutor, IEmbeddedWalletTransactionHandler
    {
        private readonly TransactionPool transactionPool;

        private readonly IEmbeddedWalletConfig config;

        public EmbeddedWalletTransactionExecutor(IEmbeddedWalletConfig config, TransactionPool transactionPool, IAccountProvider accountProvider, IRpcProvider rpcProvider)
            : base(accountProvider, rpcProvider)
        {
            this.config = config;

            this.transactionPool = transactionPool;
        }

        public event Action<EmbeddedWalletTransaction> OnTransactionQueued;

        public event Action<EmbeddedWalletTransaction> OnTransactionConfirmed;

        public override Task<TransactionResponse> SendTransaction(TransactionRequest request)
        {
            if (config.AutoApproveTransactions)
            {
                return base.SendTransaction(request);
            }

            // Add transaction to pool.
            var transaction = transactionPool.Enqueue(request);

            OnTransactionQueued?.Invoke(transaction);

            return transaction.Response.Task;
        }

        public async void TransactionApproved()
        {
            var transaction = transactionPool.Dequeue();

            try
            {
                var response = await base.SendTransaction(transaction.Request);

                transaction.Response.SetResult(response);

                OnTransactionConfirmed?.Invoke(transaction);
            }
            catch (Exception e)
            {
                transaction.Response.SetException(e);
            }
        }

        public void TransactionDeclined()
        {
            var transaction = transactionPool.Dequeue();

            transaction.Response.SetCanceled();
        }
    }
}