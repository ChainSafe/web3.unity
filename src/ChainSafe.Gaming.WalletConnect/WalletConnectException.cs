using System;
using ChainSafe.Gaming.Web3;

namespace ChainSafe.Gaming.WalletConnect
{
    /// <summary>
    /// Represents exception related to the WalletConnect integration.
    /// </summary>
    public class WalletConnectException : Web3Exception
    {
        public WalletConnectException(string message)
            : base(message)
        {
        }

        public WalletConnectException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}