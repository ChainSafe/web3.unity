using System;
using System.Collections.Generic;

namespace Models
{
    public class MintedNFT
    {
        [Serializable]
        public class Response
        {
            public string creator;
            public string owner;
            public string id;
            public string nftContract;
            public bool isApproved;
            public string tokenType;
            public string uri;
        }
        [Serializable]
        public class Root
        {
            public List<Response> response;
        }
    }
}