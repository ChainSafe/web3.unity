using System;

namespace ChainSafe.Gaming.Debug
{
    public class AssertionException : Exception
    {
        public AssertionException(string message)
            : base(message)
        {
        }
    }
}