// <copyright file="IMarketplaceConfig.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Reflection;
using ChainSafe.Gaming.Evm.Utils;

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
        string MarketplaceContractAbiOverride { get; set; }

        string ProjectIdOverride { get; set; }

        string MarketplaceId { get; set; }

        string MarketplaceContractAddress { get; set; }

        string MarketplaceContractAbi => !string.IsNullOrEmpty(MarketplaceContractAbiOverride)
            ? MarketplaceContractAbiOverride
            : ReadDefaultAbiFromResources();

        public static string ReadDefaultAbiFromResources()
        {
            return AbiHelper.ReadAbiFromResources(Assembly.GetExecutingAssembly(), "ChainSafe.Gaming.Marketplace.marketplace-abi.json");
        }
    }
}