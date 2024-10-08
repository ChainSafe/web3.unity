// <copyright file="MarketplaceConfig.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;

namespace ChainSafe.Gaming.Marketplace
{
    /// <summary>
    /// Marketplace configuration.
    /// </summary>
    [Serializable]
    public class MarketplaceConfig : IMarketplaceConfig
    {
        /// <inheritdoc/>
        public string? EndpointOverride { get; set; }

        /// <inheritdoc/>
        public string MarketplaceContractAbiOverride { get; set; }

        public string ProjectIdOverride { get; set; }

        public string MarketplaceId { get; set; }

        public string MarketplaceContractAddress { get; set; }
    }
}