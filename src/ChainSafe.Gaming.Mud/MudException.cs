using System;
using ChainSafe.Gaming.Web3;

namespace ChainSafe.Gaming.Mud
{
    public class MudException : Web3Exception
    {
        public MudException(string message)
            : base(message)
        {
        }

        public MudException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}