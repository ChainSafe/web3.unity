// <copyright file="MarketplaceItem.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ChainSafe.Gaming.Marketplace
{
    using System;
    using Newtonsoft.Json;

    /// <summary>
    /// Marketplace items.
    /// </summary>
    public class MarketplaceItem
    {
        /// <summary>
        /// Gets or sets Id.
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets chain Id.
        /// </summary>
        [JsonProperty(PropertyName = "chain_id")]
        public int ChainId { get; set; }

        /// <summary>
        /// Gets or sets project Id.
        /// </summary>
        [JsonProperty(PropertyName = "project_id")]
        public string ProjectId { get; set; }

        /// <summary>
        /// Gets or sets marketplace Id.
        /// </summary>
        [JsonProperty(PropertyName = "marketplace_id")]
        public string MarketplaceId { get; set; }

        /// <summary>
        /// Gets or sets token.
        /// </summary>
        [JsonProperty(PropertyName = "token")]
        public MarketplaceItemToken Token { get; set; }

        /// <summary>
        /// Gets or sets the marketplace contract address.
        /// </summary>
        [JsonProperty(PropertyName = "marketplace_contract_address")]
        public string MarketPlaceContractAddress { get; set; }

        /// <summary>
        /// Gets or sets seller.
        /// </summary>
        [JsonProperty(PropertyName = "seller")]
        public string Seller { get; set; }

        /// <summary>
        /// Gets or sets buyer.
        /// </summary>
        [JsonProperty(PropertyName = "buyer")]
        public string Buyer { get; set; }

        /// <summary>
        /// Gets or sets price.
        /// </summary>
        [JsonProperty(PropertyName = "price")]
        public string Price { get; set; }

        /// <summary>
        /// Gets or sets listed at time.
        /// </summary>
        [JsonProperty(PropertyName = "listed_at")]
        public long ListedAtRaw { get; set; }

        /// <summary>
        /// Gets or sets statusRaw for use with item status.
        /// </summary>
        [JsonProperty(PropertyName = "status")]
        public string StatusRaw { get; set; }

        /// <summary>
        /// Gets listed at time data.
        /// </summary>
        public DateTime ListedAt => DateTime.UnixEpoch + TimeSpan.FromMilliseconds(this.ListedAtRaw);

        /// <summary>
        /// Gets the status of marketplace items.
        /// </summary>
        public MarketplaceItemStatus Status => MarketplaceItemStatusExtensions.FromString(this.StatusRaw);
    }
}