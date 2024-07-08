using System;
using ChainSafe.Gaming.Evm.Transactions;

namespace ChainSafe.GamingSdk.Web3Auth
{
    public class Web3AuthTransactionHandler : IWeb3AuthTransactionHandler
    {
        public event Action<TransactionRequested> OnTransactionRequested;
        
        public event Action<TransactionConfirmed> OnTransactionConfirmed;
        
        public event Action<TransactionApproved> OnTransactionApproved; 
        
        public event Action<TransactionDeclined> OnTransactionDeclined;

        public void RequestTransaction(TransactionRequested transactionRequested)
        {
            OnTransactionRequested?.Invoke(transactionRequested);
        }

        public void ConfirmTransaction(TransactionConfirmed transactionConfirmed)
        {
            OnTransactionConfirmed?.Invoke(transactionConfirmed);
        }

        public void ApproveTransaction(TransactionApproved transactionApproved)
        {
            OnTransactionApproved?.Invoke(transactionApproved);
        }
        
        public void DeclineTransaction(TransactionDeclined transactionDeclined)
        {
            OnTransactionDeclined?.Invoke(transactionDeclined);
        }
    }
}