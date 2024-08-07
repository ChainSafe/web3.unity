using System.Collections.Generic;
using Newtonsoft.Json;
using BigInteger = System.Numerics.BigInteger;

namespace ChainSafe.Gaming.UnityPackage.Model
{
    public class MarketplaceModel
    {
        #region ProjectMarketplaces

        public class ProjectMarketplacesResponse
        {
            [JsonProperty("page_number")]
            public int PageNumber { get; set; }

            [JsonProperty("page_size")]
            public int PageSize { get; set; }

            [JsonProperty("total")]
            public int Total { get; set; }

            [JsonProperty("cursor")]
            public string Cursor { get; set; }

            [JsonProperty("marketplaces")]
            public List<Marketplace> Marketplaces { get; set; }
        }

        public class Marketplace
        {
            public string id { get; set; }
            public string name { get; set; }
            public string description { get; set; }
            public string owner { get; set; }
            public long chain_id { get; set; }
            public string banner { get; set; }
            public string project_id { get; set; }
            public string contract_address { get; set; }
            public bool deployed { get; set; }
            public long created_at { get; set; }
            public long updated_at { get; set; }
        }

        #endregion

        #region MarketplaceItems

        public class MarketplaceItemsResponse
        {
            public int page_number { get; set; }
            public int page_size { get; set; }
            public int total { get; set; }
            public string cursor { get; set; }
            public List<Item> items { get; set; }
            public List<Owners> owners { get; set; }
        }

        public class Item
        {
            public string id { get; set; }
            public string chain_id { get; set; }
            public string project_id { get; set; }
            public string marketplace_id { get; set; }
            public Token token { get; set; }
            public string marketplace_contract_address { get; set; }
            public string seller { get; set; }
            public string buyer { get; set; }
            public string price { get; set; }
            public BigInteger listed_at { get; set; }
            public string status { get; set; }
        }

        public class Token
        {
            public string token_id { get; set; }
            public string token_type { get; set; }
            public string contract_address { get; set; }
            public string uri { get; set; }
            public Metadata metadata { get; set; }
        }

        public class Metadata
        {
            public List<Attribute> attributes { get; set; }
        }

        public class Attribute
        {
            public string trait_type { get; set; }
            public string value { get; set; }
        }

        public class Owners
        {
            public string owner { get; set; }
            public string supply { get; set; }
        }

        #endregion

    }
}