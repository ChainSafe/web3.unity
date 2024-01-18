using System;
using ChainSafe.Gaming.Web3;

namespace ChainSafe.Gaming.WalletConnect
{
    public class WalletConnectException : Web3Exception
    {
        public WalletConnectException(string message) : base(message)
        {
        }

        public WalletConnectException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}