using System;
using System.Numerics;

namespace ChainSafe.Gaming.UnityPackage.Model
{
    public class GetVoucherModel
    {
        [Serializable]
        public class GetVoucher721Response
        {
            public int minPrice;
            public string tokenId;
            public string signer;
            public string receiver;
            public string signature;
        }
        [Serializable]
        public class GetVoucher1155Response
        {
            public int minPrice;
            public string tokenId;
            public int amount;
            public ulong nonce;
            public string signer;
            public string receiver;
            public string signature;
        }
    }
}