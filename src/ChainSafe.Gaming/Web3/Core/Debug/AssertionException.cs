using System;

namespace ChainSafe.Gaming.Web3.Core.Debug
{
    public class AssertionException : Exception
    {
        public AssertionException(string message)
            : base(message)
        {
        }
    }
}