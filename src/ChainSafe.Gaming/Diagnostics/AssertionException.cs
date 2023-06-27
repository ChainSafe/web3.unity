using System;

namespace ChainSafe.Gaming.Diagnostics
{
    public class AssertionException : Exception
    {
        public AssertionException(string message)
            : base(message)
        {
        }
    }
}