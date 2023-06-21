﻿using System;

namespace ChainSafe.Gaming
{
    public class Web3Exception : Exception
    {
        public Web3Exception(string message)
            : base(message)
        {
        }

        public Web3Exception(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}