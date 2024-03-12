﻿// <copyright file="MarketplacePage.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ChainSafe.Gaming.Marketplace
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    /// <summary>
    /// Marketplace page.
    /// </summary>
    public class MarketplacePage
    {
        /// <summary>
        /// Gets & sets page number.
        /// </summary>
        [JsonProperty(PropertyName = "page_number")]
        public int PageNumber { get; set; }

        /// <summary>
        /// Gets & sets page size.
        /// </summary>
        [JsonProperty(PropertyName = "page_size")]
        public int PageSize { get; set; }

        /// <summary>
        /// Gets & sets total.
        /// </summary>
        [JsonProperty(PropertyName = "total")]
        public int Total { get; set; }

        /// <summary>
        /// Gets & sets cursor.
        /// </summary>
        [JsonProperty(PropertyName = "cursor")]
        public string Cursor { get; set; }

        /// <summary>
        /// Gets & sets items.
        /// </summary>
        [JsonProperty(PropertyName = "items")]
        public List<MarketplaceItem> Items { get; set; }
    }
}