using System.Threading.Tasks;
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
        
        #endregion

        #region Events
        
        public delegate void TransactionRequested(TransactionRequest request);

        public static event TransactionRequested OnTransactionRequested;
        
        public delegate void TransactionResponded(TransactionResponse response);

        public static event TransactionResponded OnTransactionResponse;
        
        public delegate void TransactionAccepted();

        public static event TransactionAccepted OnTransactionAccepted;
        
        public delegate void TransactionRejected();

        public static event TransactionRejected OnTransactionRejected;

        #endregion

        #region Methods
        
        /// <summary>
        /// Static constructor for event handlers.
        /// </summary>
        static Web3AuthTransactionHelper()
        {
            OnTransactionRequested = (request) =>
            {
                StoredTransactionRequest = request;
                Web3AuthEventManager.RaiseIncomingTransaction();
            };
            OnTransactionResponse = (response) =>
            {
                StoredTransactionResponse = response;
            };
            OnTransactionAccepted = () =>
            {
                TransactionAcceptedTcs?.SetResult(true);
                TransactionAcceptedTcs = null;
            };
            OnTransactionRejected = () =>
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
        
        /// <summary>
        /// Invokes transaction requested.
        /// </summary>
        /// <param name="request">The transaction request</param>
        public static void InvokeTransactionRequested(TransactionRequest request)
        {
            OnTransactionRequested?.Invoke(request);
        }
        
        /// <summary>
        /// Invokes transaction response.
        /// </summary>
        /// <param name="response">The transaction response</param>
        public static void InvokeTransactionResponded(TransactionResponse response)
        {
            OnTransactionResponse?.Invoke(response);
        }
        
        /// <summary>
        /// Invokes transaction accepted.
        /// </summary>
        public static void InvokeTransactionAccepted()
        {
            OnTransactionAccepted?.Invoke();
        }
        
        /// <summary>
        /// Invokes transaction rejected.
        /// </summary>
        public static void InvokeTransactionRejected()
        {
            OnTransactionRejected?.Invoke();
        }
        
        #endregion
    }
}