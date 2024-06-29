using System.Collections.Generic;

namespace ChainSafe.Gaming.UnityPackage.Model
{
    public class NftTokenModel
    {
        #region ProjectCollections
        
        public class ProjectCollectionsResponse
        {
            public int PageSize { get; set; }
            public int Total { get; set; }
            public string Cursor { get; set; }
            public List<Collection> Collections { get; set; }
        }
        
        public class Collection
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public string Owner { get; set; }
            public int ChainID { get; set; }
            public string ProjectID { get; set; }
            public string ContractAddress { get; set; }
            public bool Deployed { get; set; }
            public bool IsImported { get; set; }
            public string Type { get; set; }
            public string Logo { get; set; }
            public string Banner { get; set; }
            public long CreatedAt { get; set; }
            public long UpdatedAt { get; set; }
        }

        #endregion

        #region CollectionsItems
        
        public class CollectionItemsResponse
        {
            public int PageNumber { get; set; }
            public int PageSize { get; set; }
            public int Total { get; set; }
            public string Cursor { get; set; }
            public List<Token> Tokens { get; set; }
        }

        public class Token
        {
            public string TokenID { get; set; }
            public string ChainID { get; set; }
            public string ProjectID { get; set; }
            public string CollectionID { get; set; }
            public string TokenType { get; set; }
            public string ContractAddress { get; set; }
            public string Supply { get; set; }
            public string Uri { get; set; }
            public Metadata Metadata { get; set; }
        }
        
        public class Metadata
        {
            public List<Attribute> Attributes { get; set; }
        }
        
        public class Attribute
        {
            public string TraitType { get; set; }
            public string Value { get; set; }
        }
        
        #endregion
        
    }
}
