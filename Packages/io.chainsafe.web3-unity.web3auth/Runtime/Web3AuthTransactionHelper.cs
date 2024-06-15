using System.Threading.Tasks;
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

        public static bool Working { get; set; }

        #endregion

        #region Methods
        
        /// <summary>
        /// Waits for transcation confirmation.
        /// </summary>
        public static async Task WaitForTransactionAsync()
        {
            Debug.Log("Waiting for Web3AuthWallet tx confirmation");
            TransactionAcceptedTcs = new TaskCompletionSource<bool>();
            Working = true;
            await TransactionAcceptedTcs.Task;
        }
        
        /// <summary>
        /// Accepts a pending transcation.
        /// </summary>
        public static void AcceptTransaction()
        {
            Debug.Log("Accepting Web3AuthWallet transaction");
            TransactionAcceptedTcs.SetResult(true);
            Working = false;
            TransactionAcceptedTcs = null;
        }
        
        /// <summary>
        /// Rejects a pending transcation.
        /// </summary>
        public static void RejectTransaction()
        {
            Debug.Log("Rejecting Web3AuthWallet transaction");
            TransactionAcceptedTcs.SetResult(false);
            Working = false;
        }
        
        #endregion
    }
}