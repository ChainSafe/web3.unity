// <copyright file="MarketplaceExtensions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ChainSafe.Gaming.Marketplace.Extensions
{
    using ChainSafe.Gaming.Web3.Build;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;

    /// <summary>
    /// Marketplace extensions.
    /// </summary>
    public static class MarketplaceExtensions
    {
        /// <summary>
        /// Configures the marketplace.
        /// </summary>
        /// <param name="services">Services.</param>
        /// <param name="config">Config.</param>
        /// <returns>Service with config.</returns>
        public static IWeb3ServiceCollection ConfigureMarketplace(
            this IWeb3ServiceCollection services,
            IMarketplaceConfig config)
        {
            services.Replace(ServiceDescriptor.Singleton(typeof(IMarketplaceConfig), config));
            return services;
        }

        /// <summary>
        /// Uses the marketplace service.
        /// </summary>
        /// <param name="services">Services.</param>
        /// <returns>Services with singleton marketplace client.</returns>
        public static IWeb3ServiceCollection UseMarketplace(this IWeb3ServiceCollection services)
        {
            services.AssertServiceNotBound<MarketplaceClient>();
            services.AddSingleton<MarketplaceClient>();
            return services;
        }

        /// <summary>
        /// Uses the marketplace service with config.
        /// </summary>
        /// <param name="services">Services.</param>
        /// <param name="config">Config.</param>
        /// <returns>Services with config.</returns>
        public static IWeb3ServiceCollection UseMarketplace(
            this IWeb3ServiceCollection services,
            IMarketplaceConfig config)
        {
            services.AssertServiceNotBound<MarketplaceClient>();
            services.ConfigureMarketplace(config);
            services.UseMarketplace();
            return services;
        }

        /// <summary>
        /// Gets the marketplace client service.
        /// </summary>
        /// <param name="web3">Web3.</param>
        /// <returns>Marketplace client service.</returns>
        public static MarketplaceClient Marketplace(this Web3.Web3 web3)
        {
            return web3.ServiceProvider.GetRequiredService<MarketplaceClient>();
        }
    }
}