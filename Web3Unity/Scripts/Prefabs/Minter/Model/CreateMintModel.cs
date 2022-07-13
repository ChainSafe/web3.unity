using System;

namespace Models
{
    public class CreateMintModel
    {
        [Serializable]
        public class Connection
        {
            public string chain;
            public string network;
        }

        [Serializable]
        public class Response
        {
            public string hashedUnsignedTx;
            public Connection connection;
            public string cid;
            public Tx tx;
        }

        [Serializable]
        public class Root
        {
            public Response response;

            public override string ToString()
            {
                return UnityEngine.JsonUtility.ToJson(this, true);
            }
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

    [Serializable]
    public class MintNFT
    {
        public string chain;

        public string network;

        public string account;

        public string to;
        public string cid;
        
    }
}