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
        
        public static TaskCompletionSource<bool> TransactionAcceptedTcs { get; set; }
        public static TransactionRequest StoredTransactionRequest { get; set; }
        public static TransactionResponse StoredTransactionResponse { get; set; }
        public static Action<TransactionRequest> TransactionRequest { get; set; }
        public static Action <TransactionResponse> TransactionResponse { get; set; }
        public static Action TransactionAccepted { get; set; }
        public static Action TransactionRejected { get; set; }
        private static bool TransactionProcessing { get; set; }

        #endregion

        #region Methods
        
        /// <summary>
        /// Static constructor for event handlers.
        /// </summary>
        static Web3AuthTransactionHelper()
        {
            TransactionRequest = (request) =>
            {
                StoredTransactionRequest = request;
            };
            TransactionResponse = (response) =>
            {
                StoredTransactionResponse = response;
                TransactionProcessing = false;
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
                TransactionProcessing = false;
            };
        }
        
        /// <summary>
        /// Event method, invokes transaction request event.
        /// </summary>
        /// <param name="request">Transaction request</param>
        public static void OnTransactionRequest(TransactionRequest request)
        {
            TransactionRequest?.Invoke(request);
        }
        
        /// <summary>
        /// Event method, invokes transaction response event.
        /// </summary>
        /// <param name="response">Transaction response</param>
        public static void OnTransactionResponse(TransactionResponse response)
        {
            TransactionResponse?.Invoke(response);
        }
        
        /// <summary>
        /// Event method, invokes transaction accepted event.
        /// </summary>
        public static void OnTransactionAccepted()
        {
            if (TransactionProcessing) return;
            TransactionProcessing = true;
            TransactionAccepted?.Invoke();
        }
        
        /// <summary>
        /// Event method, invokes transaction rejected event.
        /// </summary>
        public static void OnTransactionRejected()
        {
            TransactionRejected?.Invoke();
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