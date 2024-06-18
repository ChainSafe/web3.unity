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
        
        public static TransactionRequest StoredTransactionRequest { get; set; }
        public static TransactionResponse StoredTransactionResponse { get; set; }
        public static TaskCompletionSource<bool> TransactionAcceptedTcs { get; set; }
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
                StoredTransactionRequest = request;
                Console.WriteLine($"Transaction Request Stored");
            };
            TransactionResponse = (response) =>
            {
                StoredTransactionResponse = response;
                Console.WriteLine($"Transaction Response Stored");
            };
            TransactionAccepted = () =>
            {
                Console.WriteLine("Transaction Accepted");
                TransactionAcceptedTcs?.SetResult(true);
                TransactionAcceptedTcs = null;
            };
            TransactionRejected = () =>
            {
                Console.WriteLine("Transaction Rejected");
                TransactionAcceptedTcs?.SetResult(false);
                TransactionAcceptedTcs = null;
            };
        }
        
        /// <summary>
        /// Event method, called when transaction request event is invoked.
        /// </summary>
        /// <param name="request">Transaction request</param>
        public static void OnTransactionRequest(TransactionRequest request)
        {
            TransactionRequest?.Invoke(request);
        }
        
        /// <summary>
        /// Event method, called when transaction response event is invoked.
        /// </summary>
        /// <param name="response">Transaction response</param>
        public static void OnTransactionResponse(TransactionResponse response)
        {
            TransactionResponse?.Invoke(response);
        }
        
        /// <summary>
        /// Event method, called when transaction accepted event is invoked.
        /// </summary>
        public static void OnTransactionAccepted()
        {
            TransactionAccepted?.Invoke();
        }
        
        /// <summary>
        /// Event method, called when transaction rejected event is invoked.
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