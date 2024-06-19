using System.Threading.Tasks;
using System;
using ChainSafe.Gaming.Evm.Transactions;
using UnityEngine;

namespace ChainSafe.GamingSdk.Web3Auth
{
    /// <summary>
    /// Transaction helpers class to halt transaction processing when the Web3Auth wallet GUI is active.
    /// </summary>
    public static class Web3AuthTransactionHelper
    {

        #region Properties
        
        private static TaskCompletionSource<bool> TransactionAcceptedTcs { get; set; }
        public static TransactionRequest StoredTransactionRequest { get; set; }
        public static TransactionResponse StoredTransactionResponse { get; set; }
        public static Action<TransactionRequest> TransactionRequest { get; set; }
        public static Action <TransactionResponse> TransactionResponse { get; set; }
        public static Action TransactionAccepted { get; set; }
        public static Action TransactionRejected { get; set; }

        #endregion

        #region Methods
        
        /// <summary>
        /// Static constructor for event handlers.
        /// </summary>
        static Web3AuthTransactionHelper()
        {
            TransactionRequest = (request) =>
            {
                Web3AuthEventManager.RaiseIncomingTransaction();
                StoredTransactionRequest = request;
            };
            TransactionResponse = (response) =>
            {
                StoredTransactionResponse = response;
            };
            TransactionAccepted = () =>
            {
                TransactionAcceptedTcs?.SetResult(true);
                TransactionAcceptedTcs = null;
            };
            TransactionRejected = () =>
            {
                TransactionAcceptedTcs?.SetResult(false);
                TransactionAcceptedTcs = null;
            };
        }
        
        /// <summary>
        /// Waits for transaction confirmation.
        /// </summary>
        public static async Task WaitForTransactionAsync()
        {
            Debug.Log("Waiting For Web3AuthWallet TX Confirmation");
            TransactionAcceptedTcs = new TaskCompletionSource<bool>();
            await TransactionAcceptedTcs.Task;
        }
        
        #endregion
    }
}