using System;
using System.Collections.Generic;

namespace Models
{
    public class MintedNFT
    {
        [Serializable]
        public class Response
        {
            public string id;
            public string uri;
            public bool isApproved;
            public string nftContract;
            public string tokenType;
        }
        [Serializable]
        public class Root
        {
            public List<Response> response;
        }
    }
}