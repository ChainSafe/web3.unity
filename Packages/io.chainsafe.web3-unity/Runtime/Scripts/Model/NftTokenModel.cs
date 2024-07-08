using System.Collections.Generic;
using Newtonsoft.Json;

namespace ChainSafe.Gaming.UnityPackage.Model
{
    public class NftTokenModel
    {
        #region ProjectCollections
        
        public class ProjectCollectionsResponse
        {
            [JsonProperty("page_size")]
            public int PageSize { get; set; }
            
            [JsonProperty("total")]
            public int Total { get; set; }
            
            [JsonProperty("cursor")]
            public string Cursor { get; set; }
            
            [JsonProperty("collections")]
            public List<Collection> Collections { get; set; }
        }
        
        public class Collection
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
            public int ChainID { get; set; }
            
            [JsonProperty("project_id")]
            public string ProjectID { get; set; }
            
            [JsonProperty("contract_address")]
            public string ContractAddress { get; set; }
            
            [JsonProperty("deployed")]
            public bool Deployed { get; set; }
            
            [JsonProperty("is_imported")]
            public bool IsImported { get; set; }
            
            [JsonProperty("type")]
            public string Type { get; set; }
            
            [JsonProperty("logo")]
            public string Logo { get; set; }
            
            [JsonProperty("banner")]
            public string Banner { get; set; }
            
            [JsonProperty("created_at")]
            public long CreatedAt { get; set; }
            
            [JsonProperty("updated_at")]
            public long UpdatedAt { get; set; }
        }

        #endregion

        #region CollectionItems
        
        public class CollectionItemsResponse
        {
            [JsonProperty("page_number")]
            public int PageNumber { get; set; }
            
            [JsonProperty("page_size")]
            public int PageSize { get; set; }
            
            [JsonProperty("total")]
            public int Total { get; set; }
            
            [JsonProperty("cursor")]
            public string Cursor { get; set; }
            
            [JsonProperty("tokens")]
            public List<Token> Tokens { get; set; }
        }

        public class Token
        {
            [JsonProperty("token_id")]
            public string TokenID { get; set; }
            
            [JsonProperty("chain_id")]
            public string ChainID { get; set; }
            
            [JsonProperty("project_id")]
            public string ProjectID { get; set; }
            
            [JsonProperty("collection_id")]
            public string CollectionID { get; set; }
            
            [JsonProperty("token_type")]
            public string TokenType { get; set; }
            
            [JsonProperty("contract_address")]
            public string ContractAddress { get; set; }
            
            [JsonProperty("supply")]
            public string Supply { get; set; }
            
            [JsonProperty("uri")]
            public string Uri { get; set; }
            
            [JsonProperty("metadata")]
            public Metadata Metadata { get; set; }
        }

        public class Metadata
        {
            [JsonProperty("attributes")]
            public List<Attribute> Attributes { get; set; }
            
            [JsonProperty("description")]
            public string Description { get; set; }
            
            [JsonProperty("image")]
            public string Image { get; set; }
            
            [JsonProperty("name")]
            public string Name { get; set; }
            
            [JsonProperty("tokenType")]
            public string TokenType { get; set; }
        }

        public class Attribute
        {
            [JsonProperty("trait_type")]
            public string TraitType { get; set; }
            
            [JsonProperty("value")]
            public string Value { get; set; }
        }
        
        #endregion
        
    }
}
