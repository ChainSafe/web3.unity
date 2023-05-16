using System;

namespace ChainSafe.GamingWeb3.Build
{
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