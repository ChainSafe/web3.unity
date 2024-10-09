using System;

namespace ChainSafe.Gaming.Web3.Core
{
    public class ServiceNotBoundWeb3Exception<T> : Web3Exception
    {
        public ServiceNotBoundWeb3Exception(string message)
            : base(message)
        {
        }

        public ServiceNotBoundWeb3Exception(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}