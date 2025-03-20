using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Transactions;

namespace ChainSafe.Gaming.EmbeddedWallet
{
    /// <summary>
    /// Transaction object for handling embedded wallet transactions.
    /// </summary>
    public class EmbeddedWalletTransaction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmbeddedWalletTransaction"/> class.
        /// </summary>
        /// <param name="request">Initial transaction request.</param>
        public EmbeddedWalletTransaction(TransactionRequest request)
        {
            Request = request;

            Response = new TaskCompletionSource<TransactionResponse>();
        }

        /// <summary>
        /// Initial Transaction Request.
        /// </summary>
        public TransactionRequest Request { get; private set; }

        /// <summary>
        /// Awaitable Transaction Response.
        /// </summary>
        public TaskCompletionSource<TransactionResponse> Response { get; private set; }
    }
}