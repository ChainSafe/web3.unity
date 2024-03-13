// <copyright file="IMarketplaceConfig.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ChainSafe.Gaming.Marketplace
{
    /// <summary>
    /// Marketplace config interface.
    /// </summary>
    public interface IMarketplaceConfig
    {
        /// <summary>
        /// Gets or sets end point override.
        /// </summary>
        string EndpointOverride { get; set; }

        /// <summary>
        /// Gets or sets marketplace contract abi.
        /// </summary>
        string? MarketplaceContractAbi { get; set; }
    }
}