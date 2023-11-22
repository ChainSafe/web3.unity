using System;

namespace ChainSafe.Gaming.Web3
{
    /// <summary>
    /// Wallet related exception.
    /// </summary>
    public class WalletException : Exception
    {
        public WalletException(string message)
            : base(message)
        {
        }

        public WalletException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}