using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Models
{
    public class BuyNFT
    {
        [Serializable]
        public class Connection
        {
            public string rpc;
            public string chain;
            public string network;
        }
        [Serializable]
        public class Response
        {
            public string hashedUnsignedTx;
            public Connection connection;
            public Tx tx;
        }
        [Serializable]
        public class Root
        {
            public Response response;
        }
        [Serializable]
        public class Tx
        {
            public string account;
            public string to;
            public string value;
            public string data;
            public string gasPrice;
            public string gasLimit;
        }
    }
}
