using System;

namespace ChainSafe.Gaming.EmbeddedWallet
{
    /// <summary>
    /// Handles Transaction requests, approvals, declines and confirmations.
    /// </summary>
    public interface IEmbeddedWalletTransactionHandler
    {
        /// <summary>
        /// Invokes when transaction is requested.
        /// </summary>
        public event Action<EmbeddedWalletTransaction> OnTransactionQueued;

        /// <summary>
        /// Invokes when transaction is confirmed on block.
        /// </summary>
        public event Action<EmbeddedWalletTransaction> OnTransactionConfirmed;

        /// <summary>
        /// Transaction got approved.
        /// </summary>
        public void TransactionApproved();

        /// <summary>
        /// Transaction got declined.
        /// </summary>
        public void TransactionDeclined();
    }
}