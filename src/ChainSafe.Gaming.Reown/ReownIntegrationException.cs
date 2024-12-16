using System;
using ChainSafe.Gaming.Web3;

namespace ChainSafe.Gaming.Reown
{
    /// <summary>
    /// Represents exception related to the Reown integration.
    /// </summary>
    public class ReownIntegrationException : Web3Exception
    {
        public ReownIntegrationException(string message)
            : base(message)
        {
        }

        public ReownIntegrationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}