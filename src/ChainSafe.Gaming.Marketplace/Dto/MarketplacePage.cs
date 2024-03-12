// <copyright file="MarketplacePage.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ChainSafe.Gaming.Marketplace
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class MarketplacePage
    {
        /// <summary>
        /// Page number
        /// </summary>
        [JsonProperty(PropertyName = "page_number")]
        public int PageNumber { get; set; }

        /// <summary>
        /// Page size
        /// </summary>
        [JsonProperty(PropertyName = "page_size")]
        public int PageSize { get; set; }

        /// <summary>
        /// Total
        /// </summary>
        [JsonProperty(PropertyName = "total")]
        public int Total { get; set; }

        /// <summary>
        /// Cursor
        /// </summary>
        [JsonProperty(PropertyName = "cursor")]
        public string Cursor { get; set; }

        /// <summary>
        /// Items
        /// </summary>
        [JsonProperty(PropertyName = "items")]
        public List<MarketplaceItem> Items { get; set; }
    }
}