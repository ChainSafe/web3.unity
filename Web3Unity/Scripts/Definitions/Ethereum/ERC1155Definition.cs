using System;
using System.Collections.Generic;

namespace ERC1155Definition
{
    [Serializable]
    public class BalanceOf
    {
        public string balanceOf;
    }

    [Serializable]
    public class BalanceOfBatch
    {
        public List<string> balanceOfBatch;
    }

    [Serializable]
    public class Uri
    {
        public string uri;
    }

    [Serializable]
    public class IsApprovedForAll
    {
        public bool isApprovedForAll;
    }
}
