using ChainSafe.Gaming.Evm.Transactions;

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
        
        #endregion

        #region Events
        
        public delegate void TransactionAccepted();

        public static event TransactionAccepted OnTransactionAccepted;
        
        public delegate void TransactionRejected();

        public static event TransactionRejected OnTransactionRejected;

        #endregion

        #region Methods
        
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