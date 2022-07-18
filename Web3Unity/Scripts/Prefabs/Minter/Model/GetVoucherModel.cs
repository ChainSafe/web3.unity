using System;

namespace Models
{
    public class GetVoucherModel
    {
        [Serializable]
        public class GetVoucher721Response
        {
            public int minPrice;
            public string uri;
            public string signer;
            public string signature;
        }
        [Serializable]
        public class GetVoucher1155Response
        {
            public int minPrice;
            public string tokenId;
            public int nonce;
            public string signer;
            public string signature;
        }
    }
}