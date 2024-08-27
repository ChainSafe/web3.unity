using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Contracts.GasFees;
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
        /// <param name="gasFeeModifier">
        /// Optional. If <c>null</c>, the default is an instance of <see cref="NoGasFeeModifier"/>.
        /// Instantiate one of the gas fee modifiers if you want to customize the gas fees for a specific transaction.
        /// </param>
        /// </returns>
        public Task<TransactionResponse> SendTransaction(TransactionRequest transaction, IGasFeeModifier gasFeeModifier = null);
    }
}