using System;
using ChainSafe.Gaming.Evm.Transactions;

namespace ChainSafe.GamingSdk.Web3Auth
{
    public interface IWeb3AuthTransactionHandler
    {
        public event Action<TransactionRequested> OnTransactionRequested;

        public event Action<TransactionConfirmed> OnTransactionConfirmed;

        public void RequestTransaction(TransactionRequested transactionRequested);

        public void ConfirmTransaction(TransactionConfirmed transactionConfirmed);
        
        public void ApproveTransaction(TransactionApproved transactionApproved);

        public void DeclineTransaction(TransactionDeclined transactionDeclined);
    }
}