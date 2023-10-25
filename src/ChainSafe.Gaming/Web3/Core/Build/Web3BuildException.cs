using System;
using ChainSafe.Gaming.Web3.Core;

namespace ChainSafe.Gaming.Web3.Build
{
    /// <summary>
    /// Exception that indicates an error during <see cref="Web3"/> build process.
    /// </summary>
    public class Web3BuildException : Web3Exception
    {
        public Web3BuildException(string message)
            : base(message)
        {
        }

        public Web3BuildException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}