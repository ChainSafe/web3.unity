using System;
using System.Collections.Generic;

namespace EthereumDefinition
{
    [Serializable]
    public class BalanceOf
    {
        public string balanceOf;
    }

    [Serializable]
    public class Verify
    {
        public string verify;
    }

    [Serializable]
    public class Broadcast
    {
        public string broadcast;
    }

    [Serializable]
    public class Transaction
    {
        public string network;

        public string to;

        public string wei;

        public int nonce;

        public int gasLimit;

        public string gasPrice;

        public string hex;
    }
}
