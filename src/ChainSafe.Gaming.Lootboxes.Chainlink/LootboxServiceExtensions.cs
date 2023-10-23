using Chainsafe.Gaming.Chainlink;
using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ChainSafe.Gaming.Lootboxes.Chainlink
{
    /// <summary>
    /// Extensions that are used to register the lootbox service on the consuming (i.e. Gaming/Unity) side.
    /// </summary>
    public static class LootboxServiceExtensions
    {
        /// <summary>
        /// Configures and adds the ChainlinkLootboxService to the given service collection.
        /// </summary>
        /// <param name="services">The service collection to add the Lootbox service to.</param>
        /// <param name="config">Configuration settings for the LootboxService.</param>
        /// <returns>The updated service collection with the LootboxService added.</returns>
        public static IWeb3ServiceCollection UseChainlinkLootboxService(
            this IWeb3ServiceCollection services,
            LootboxServiceConfig config)
        {
            services.AddSingleton<ILootboxService, ILifecycleParticipant, LootboxService>();
            services.Replace(ServiceDescriptor.Singleton(config));

            return services;
        }

        /// <summary>
        /// Retrieves the registered LootboxService from the ChainlinkSubCategory.
        /// </summary>
        /// <param name="chainlink">The ChainlinkSubCategory instance.</param>
        /// <returns>The registered instance of ILootboxService.</returns>
        public static ILootboxService Lootboxes(this ChainlinkSubCategory chainlink)
        {
            return ((IWeb3SubCategory)chainlink).Web3.ServiceProvider.GetRequiredService<ILootboxService>();
        }
    }
}