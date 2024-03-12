﻿// <copyright file="MarketplacePage.cs" company="ChainSafe Gaming">
// Copyright (c) ChainSafe Gaming. All rights reserved.
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
        /// Gets or sets page number.
        /// </summary>
        /// <param name="PageNumber">PageNumber.</param>
        [JsonProperty(PropertyName = "page_number")]
        public int PageNumber { get; set; }

        /// <summary>
        /// Gets or sets page size.
        /// </summary>
        [JsonProperty(PropertyName = "page_size")]
        public int PageSize { get; set; }

        /// <summary>
        /// Gets or sets total.
        /// </summary>
        [JsonProperty(PropertyName = "total")]
        public int Total { get; set; }

        /// <summary>
        /// Gets or sets cursor.
        /// </summary>
        [JsonProperty(PropertyName = "cursor")]
        public string Cursor { get; set; }

        /// <summary>
        /// Gets or sets items.
        /// </summary>
        [JsonProperty(PropertyName = "items")]
        public List<MarketplaceItem> Items { get; set; }
    }
}