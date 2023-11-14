using System;
using System.Collections.Generic;

namespace ChainSafe.Gaming.UnityPackage.Model
{
    public class AllNftModel
    {
        [Serializable]
        public class AllNfts
        {
            public int page_number;
            public int page_size;
            public int total;
            public string cursor;
            public Tokens tokens;
        }

        [Serializable]
        public class Tokens
        {
            public List<string> tokenInfo;
        }

        [Serializable]
        public class TokenInfo
        {
            public string token_id;
            public string token_type;
            public string contract_address;
            public string supply;
            public List<string> MetaDataInfo;
        }

        [Serializable]
        public class MetaData
        {
            public string description;
            public string image;
            public string name;
        }
    }
}