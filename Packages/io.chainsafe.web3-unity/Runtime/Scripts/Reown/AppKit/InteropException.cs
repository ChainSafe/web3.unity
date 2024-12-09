using System;

namespace Chainsafe.Gaming.Reown
{
    internal class InteropException : Exception
    {
        public InteropException(string message) : base(message)
        {
        }

        public InteropException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}