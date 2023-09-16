using System;
using System.Collections.Generic;

namespace Models
{
    public class CollectionModel
    {
        [Serializable]

        public class Collection
        {
            public string _id;
            public string name;
            public string slug;
            public string network;
            public string description;
            public string chain;
            public string creator;
            public List<Items> items;
            public string image;
            public string headerImage;

            [Serializable]
            public class Items
            {
                public string nftContract;
                public string tokenId;
            }
        }
        [Serializable]

        public class Response
        {
            public Collection collection;
            public string status;
        }
        [Serializable]

        public class Root
        {
            public Response response;
        }
    }
}