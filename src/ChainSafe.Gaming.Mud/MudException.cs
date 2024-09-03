using System;
using ChainSafe.Gaming.Web3;

namespace ChainSafe.Gaming.Mud
{
    /// <summary>
    /// Represents an exception that is thrown when a MUD-related error occurs.
    /// </summary>
    /// <seealso cref="Web3Exception"/>
    public class MudException : Web3Exception
    {
        internal MudException(string message)
            : base(message)
        {
        }

        internal MudException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}