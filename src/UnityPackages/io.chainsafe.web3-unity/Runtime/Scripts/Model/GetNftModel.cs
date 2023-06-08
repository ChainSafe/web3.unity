using System;
using System.Collections.Generic;

namespace Models
{
    public class GetNftModel
    {
        [Serializable]
        public class Response
        {
            public string id;
            public string uri;
            public Balances balances;
            public string owner;
            public string seller;
            public string balance;
            public bool isApproved;
            public string nftContract;
            public string tokenType;

            [Serializable]
            public class Balances
            {
                public string account;
                public string amount;
            }
        }


        [Serializable]
        public class Root
        {
            public Response response;
        }
    }
}
