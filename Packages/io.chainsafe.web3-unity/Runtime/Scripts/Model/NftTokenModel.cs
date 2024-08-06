using System.Collections.Generic;
using Newtonsoft.Json;

namespace ChainSafe.Gaming.UnityPackage.Model
{
    public class NftTokenModel
    {
        #region ProjectCollections

        public class ProjectCollectionsResponse
        {
            public int page_size { get; set; }
            public int total { get; set; }
            public string cursor { get; set; }
            public List<Collection> collections { get; set; }
        }

        public class Collection
        {
            public string id { get; set; }
            public string name { get; set; }
            public string description { get; set; }
            public string owner { get; set; }
            public int chain_id { get; set; }
            public string project_id { get; set; }
            public string contract_address { get; set; }
            public bool deployed { get; set; }
            public bool is_imported { get; set; }
            public string type { get; set; }
            public string logo { get; set; }
            public string banner { get; set; }
            public long created_at { get; set; }
            public long updated_at { get; set; }
        }

        #endregion

        #region CollectionItems

        public class CollectionItemsResponse
        {
            public int page_number { get; set; }
            public int page_size { get; set; }
            public int total { get; set; }
            public string cursor { get; set; }
            public List<Token> tokens { get; set; }
        }

        public class Token
        {
            public string token_id { get; set; }
            public string chain_id { get; set; }
            public string project_id { get; set; }
            public string collection_id { get; set; }
            public string token_type { get; set; }
            public string contract_address { get; set; }
            public string supply { get; set; }
            public string uri { get; set; }
            public Metadata metadata { get; set; }
        }

        public class Metadata
        {
            public List<Attribute> attributes { get; set; }
            public string description { get; set; }
            public string image { get; set; }
            public string name { get; set; }
            public string tokenType { get; set; }
        }

        public class Attribute
        {
            public string trait_type { get; set; }
            public string value { get; set; }
        }

        #endregion

    }
}
