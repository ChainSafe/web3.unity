using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Transactions;

namespace ChainSafe.Gaming.Web3.Core.Evm
{
    /// <summary>
    /// Represents an interface for executing blockchain transactions.
    /// </summary>
    /// <remarks>
    /// TransactionExecutor is a core component responsible for the seamless transmission of transactions to the
    /// blockchain network for execution.
    /// </remarks>
    public interface ITransactionExecutor
    {
        /// <summary>
        /// Asynchronously sends a transaction to the blockchain network.
        /// </summary>
        /// <param name="transaction">The transaction request to be sent.</param>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous operation. The task result contains
        /// a <see cref="TransactionResponse"/> object with details about the executed transaction.
        /// </returns>
        public Task<TransactionResponse> SendTransaction(TransactionRequest transaction);
    }
}