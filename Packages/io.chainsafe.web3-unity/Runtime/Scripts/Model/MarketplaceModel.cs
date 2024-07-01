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
            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("description")]
            public string Description { get; set; }

            [JsonProperty("owner")]
            public string Owner { get; set; }

            [JsonProperty("chain_id")]
            public long ChainId { get; set; }

            [JsonProperty("banner")]
            public string Banner { get; set; }

            [JsonProperty("project_id")]
            public string ProjectId { get; set; }

            [JsonProperty("contract_address")]
            public string ContractAddress { get; set; }

            [JsonProperty("deployed")]
            public bool Deployed { get; set; }

            [JsonProperty("created_at")]
            public long CreatedAt { get; set; }

            [JsonProperty("updated_at")]
            public long UpdatedAt { get; set; }
        }

        #endregion
        
        #region MarketplaceItems

        public class MarketplaceItemsResponse
        {
            [JsonProperty("page_number")]
            public int PageNumber { get; set; }

            [JsonProperty("page_size")]
            public int PageSize { get; set; }

            [JsonProperty("total")]
            public int Total { get; set; }

            [JsonProperty("cursor")]
            public string Cursor { get; set; }

            [JsonProperty("items")]
            public List<Item> Items { get; set; }

            [JsonProperty("owners")]
            public List<Owners> Owners { get; set; }
        }
        
        public class Item
        {
            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("chain_id")]
            public string ChainID { get; set; }

            [JsonProperty("project_id")]
            public string ProjectID { get; set; }

            [JsonProperty("marketplace_id")]
            public string MarketplaceID { get; set; }

            [JsonProperty("token")]
            public Token Token { get; set; }

            [JsonProperty("marketplace_contract_address")]
            public string MarketplaceContractAddress { get; set; }

            [JsonProperty("seller")]
            public string Seller { get; set; }

            [JsonProperty("buyer")]
            public string Buyer { get; set; }

            [JsonProperty("price")]
            public string Price { get; set; }

            [JsonProperty("listed_at")]
            public BigInteger ListedAt { get; set; }

            [JsonProperty("status")]
            public string Status { get; set; }
        }

        public class Token
        {
            [JsonProperty("token_id")]
            public string TokenID { get; set; }

            [JsonProperty("token_type")]
            public string TokenType { get; set; }

            [JsonProperty("contract_address")]
            public string ContractAddress { get; set; }

            [JsonProperty("uri")]
            public string Uri { get; set; }

            [JsonProperty("metadata")]
            public Metadata Metadata { get; set; }
        }    
        
        public class Metadata
        {
            [JsonProperty("attributes")]
            public List<Attribute> Attributes { get; set; }
        }

        public class Attribute
        {
            [JsonProperty("trait_type")]
            public string TraitType { get; set; }

            [JsonProperty("value")]
            public string Value { get; set; }
        }
        
        public class Owners
        {
            [JsonProperty("owner")]
            public string Owner { get; set; }
            
            [JsonProperty("supply")]
            public string Supply { get; set; }
        }
        
        #endregion
        
    }
}