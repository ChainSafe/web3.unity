using Chainsafe.Gaming.Chainlink;
using ChainSafe.GamingSDK.EVM.Web3.Core;
using ChainSafe.GamingWeb3.Build;
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

        public static ILootboxService Lootboxes(this ChainlinkClient chainlink)
        {
            return ((IWeb3Container)chainlink).Web3.ServiceProvider.GetRequiredService<ILootboxService>();
        }
    }
}