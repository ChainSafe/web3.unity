// <copyright file="MarketplaceItem.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ChainSafe.Gaming.Marketplace
{
    using System;
    using Newtonsoft.Json;
    
    public class MarketplaceItem
    {
        /// <summary>
        /// Id
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        /// <summary>
        /// Chain Id
        /// </summary>
        [JsonProperty(PropertyName = "chain_id")]
        public int ChainId { get; set; }

        /// <summary>
        /// Project Id
        /// </summary>
        [JsonProperty(PropertyName = "project_id")]
        public string ProjectId { get; set; }

        /// <summary>
        /// Marketplace Id
        /// </summary>
        [JsonProperty(PropertyName = "marketplace_id")]
        public string MarketPlaceId { get; set; }

        /// <summary>
        /// Token
        /// </summary>
        [JsonProperty(PropertyName = "token")]
        public MarketplaceItemToken Token { get; set; }

        /// <summary>
        /// The marketplace contract address
        /// </summary>
        [JsonProperty(PropertyName = "marketplace_contract_address")]
        public string MarketPlaceContractAddress { get; set; }

        /// <summary>
        /// Seller
        /// </summary>
        [JsonProperty(PropertyName = "seller")]
        public string Seller { get; set; }

        /// <summary>
        /// Buyer
        /// </summary>
        [JsonProperty(PropertyName = "buyer")]
        public string Buyer { get; set; }

        /// <summary>
        /// Price
        /// </summary>
        [JsonProperty(PropertyName = "price")]
        public string Price { get; set; }

        /// <summary>
        /// Listed at
        /// </summary>
        [JsonProperty(PropertyName = "listed_at")]
        public long ListedAtRaw { get; set; }

        /// <summary>
        /// StatusRaw
        /// </summary>
        [JsonProperty(PropertyName = "status")]
        public string StatusRaw { get; set; }

        /// <summary>
        /// Get listed at data
        /// </summary>
        public DateTime ListedAt => DateTime.UnixEpoch + TimeSpan.FromMilliseconds(this.ListedAtRaw);

        /// <summary>
        /// Gets the status of marketplace items
        /// </summary>
        public MarketplaceItemStatus Status
        {
            get
            {
                switch (this.StatusRaw)
                {
                    case "sold": return MarketplaceItemStatus.Sold;
                    case "listed": return MarketplaceItemStatus.Listed;
                    case "canceled": return MarketplaceItemStatus.Canceled;
                    default: throw new ArgumentOutOfRangeException(nameof(this.StatusRaw));
                }
            }
        }
    }
}