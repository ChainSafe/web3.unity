using System.Collections.Generic;
using BigInteger = System.Numerics.BigInteger;

namespace ChainSafe.Gaming.UnityPackage.Model
{
    public class MarketplaceModel
    {
        #region ProjectMarketplaces

        public class ProjectMarketplacesResponse
        {
            public int PageNumber { get; set; }
            public int PageSize { get; set; }
            public int Total { get; set; }
            public string Cursor { get; set; }
            public List<Marketplace> Marketplaces { get; set; }
        }

        public class Marketplace
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public string Owner { get; set; }
            public long ChainId { get; set; }
            public string Banner { get; set; }
            public string ProjectId { get; set; }
            public string ContractAddress { get; set; }
            public bool Deployed { get; set; }
            public long CreatedAt { get; set; }
            public long UpdatedAt { get; set; }
        }

        #endregion
        
        #region MarketplaceItems

        public class MarketplaceItemsResponse
        {
            public int PageNumber { get; set; }
            public int PageSize { get; set; }
            public int Total { get; set; }
            public string Cursor { get; set; }
            public List<Item> Items { get; set; }
            public List<Owners> Owners { get; set; }
        }
        
        public class Item
        {
            public string Id { get; set; }
            public string ChainID { get; set; }
            public string ProjectID { get; set; }
            public string MarketplaceID { get; set; }
            public Token Token { get; set; }
            public string MarketplaceContractAddress { get; set; }
            public string Seller { get; set; }
            public string Buyer { get; set; }
            public string Price { get; set; }
            public BigInteger ListedAt { get; set; }
            public string Status { get; set; }
        }

        public class Token
        {
            public string TokenID { get; set; }
            public string TokenType { get; set; }
            public string ContractAddress { get; set; }
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
        
        public class Owners
        {
            public string Owner { get; set; }
            public string Supply { get; set; }
        }
        
        #endregion
        
    }
}