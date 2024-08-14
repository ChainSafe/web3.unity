using System;
using ChainSafe.Gaming.Web3;

namespace ChainSafe.Gaming.Mud
{
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