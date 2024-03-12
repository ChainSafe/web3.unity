// <copyright file="MarketplaceConfig.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ChainSafe.Gaming.Marketplace
{
    /// <summary>
    /// Marketplace config.
    /// </summary>
    public struct MarketplaceConfig : IMarketplaceConfig
    {
        /// <summary>
        /// Gets new marketplace config with an endpoint override.
        /// </summary>
        public static MarketplaceConfig Default => new MarketplaceConfig
        {
            EndpointOverride = "https://api.gaming.chainsafe.io",
        };

        /// <summary>
        /// Gets or sets end point override.
        /// </summary>
        public string? EndpointOverride { get; set; }
    }
}