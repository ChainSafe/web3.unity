using System;
using System.Collections.Generic;

namespace Models
{
    public class GetNftListModel
    {
        [Serializable]
        public class Response
        {
            public string itemId;
            public string nftContract;
            public string tokenId;
            public string seller;
            public string price;
            public string listedPercentage;
            public string uri;
            public string tokenType;
        }
        [Serializable]
        public class Root
        {
            public List<Response> response;
        }

    }
}
