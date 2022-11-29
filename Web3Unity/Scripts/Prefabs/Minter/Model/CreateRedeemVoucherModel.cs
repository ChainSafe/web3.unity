using System;

namespace Models
{
    public class CreateRedeemVoucherModel
    {
        [Serializable]
        public class CreateVoucher721
        {
            public string tokenId;
            public int minPrice;
            public string signer;
            public string receiver;
            public string signature;
        }
  
        [Serializable]
        public class CreateVoucher1155
        {
            public string tokenId;
            public int minPrice;
            public string signer;
            public string receiver;
            public int amount;
            public ulong nonce;
            public string signature;
        }
    }
}