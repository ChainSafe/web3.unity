// <copyright file="MarketplacePage.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ChainSafe.Gaming.Marketplace
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class MarketplacePage
    {
        [JsonProperty(PropertyName = "page_number")]
        public int PageNumber { get; set; }

        [JsonProperty(PropertyName = "page_size")]
        public int PageSize { get; set; }

        [JsonProperty(PropertyName = "total")]
        public int Total { get; set; }

        [JsonProperty(PropertyName = "cursor")]
        public string Cursor { get; set; }

        [JsonProperty(PropertyName = "items")]
        public List<MarketplaceItem> Items { get; set; }
    }
}