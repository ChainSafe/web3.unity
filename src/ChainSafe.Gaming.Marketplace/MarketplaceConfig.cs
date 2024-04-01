// <copyright file="MarketplaceConfig.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ChainSafe.Gaming.Marketplace
{
    using System.Reflection;
    using ChainSafe.Gaming.Evm.Utils;

    /// <summary>
    /// Marketplace configuration.
    /// </summary>
    public struct MarketplaceConfig : IMarketplaceConfig
    {
        /// <summary>
        /// Gets constructor instantiates the class.
        /// </summary>
        public static MarketplaceConfig Default => new MarketplaceConfig
        {
            EndpointOverride = "https://api.gaming.chainsafe.io",
            MarketplaceContractAbi = ReadDefaultAbiFromResources(),
        };

        /// <inheritdoc/>
        public string? EndpointOverride { get; set; }

        /// <inheritdoc/>
        public string? MarketplaceContractAbi { get; set; }

        private static string ReadDefaultAbiFromResources()
        {
            return AbiHelper.ReadAbiFromResources(Assembly.GetExecutingAssembly(), "ChainSafe.Gaming.Marketplace.marketplace-abi.json");
        }
    }
}