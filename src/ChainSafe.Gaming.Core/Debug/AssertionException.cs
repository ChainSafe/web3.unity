using System;

namespace ChainSafe.GamingSDK.EVM.Web3.Core.Debug
{
    public class AssertionException : Exception
    {
        public AssertionException(string message)
            : base(message)
        {
        }
    }
}