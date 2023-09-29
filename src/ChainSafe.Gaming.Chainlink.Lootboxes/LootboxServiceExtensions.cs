using Chainsafe.Gaming.Chainlink;
using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ChainSafe.Gaming.Chainlink.Lootboxes
{
    public static class LootboxServiceExtensions
    {
        public static IWeb3ServiceCollection UseChainlinkLootboxService(
            this IWeb3ServiceCollection services,
            LootboxServiceConfig config)
        {
            services.AddSingleton<ILootboxService, ILifecycleParticipant, LootboxService>();
            services.Replace(ServiceDescriptor.Singleton(config));

            return services;
        }

        public static ILootboxService Lootboxes(this ChainlinkSubCategory chainlink)
        {
            return ((IWeb3SubCategory)chainlink).Web3.ServiceProvider.GetRequiredService<ILootboxService>();
        }
    }
}