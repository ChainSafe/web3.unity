using System;
using ChainSafe.Gaming.Evm.Transactions;

namespace ChainSafe.GamingSdk.Web3Auth
{
    /// <summary>
    /// Handles web3Auth Transaction requests, approvals, declines and confirmations.
    /// </summary>
    public interface IWeb3AuthTransactionHandler
    {
        /// <summary>
        /// Invokes when transaction is requested.
        /// </summary>
        public event Action<TransactionRequest> OnTransactionRequested;

        /// <summary>
        /// Invokes when transaction is confirmed on block.
        /// </summary>
        public event Action<TransactionResponse> OnTransactionConfirmed;

        /// <summary>
        /// Transaction got approved.
        /// </summary>
        /// <param name="transactionId">Transaction pool Id of Transaction that was approved.</param>
        public void TransactionApproved(string transactionId);

        /// <summary>
        /// Transaction got declined.
        /// </summary>
        /// <param name="transactionId">Transaction pool Id of Transaction that was declined.</param>
        public void TransactionDeclined(string transactionId);
    }
}