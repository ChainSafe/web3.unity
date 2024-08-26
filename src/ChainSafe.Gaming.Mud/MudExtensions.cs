using ChainSafe.Gaming.Mud.Storages;
using ChainSafe.Gaming.Mud.Storages.InMemory;
using ChainSafe.Gaming.Mud.Worlds;
using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Core.Nethereum;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ChainSafe.Gaming.Mud
{
    public static class MudExtensions
    {
        public static IWeb3ServiceCollection UseMud(this IWeb3ServiceCollection services, IMudConfig mudConfig)
        {
            services.AssertServiceNotBound<MudFacade>();
            services.AssertConfigurationNotBound<IMudConfig>();

            services.ConfigureMud(mudConfig);
            services.UseMud();

            return services;
        }

        public static IWeb3ServiceCollection ConfigureMud(this IWeb3ServiceCollection services, IMudConfig mudConfig)
        {
            services.Replace(ServiceDescriptor.Singleton(mudConfig));

            return services;
        }

        public static IWeb3ServiceCollection UseMud(this IWeb3ServiceCollection services)
        {
            services.AssertServiceNotBound<MudFacade>();

            services.AddSingleton<MudFacade>();
            services.AddSingleton<MudWorldFactory>();
            services.AddSingleton<IMudStorageFactory, MudStorageFactory>();

            // Storage strategies
            services.AddTransient<InMemoryMudStorage>();
            // todo implement OffchainIndexerMudStorage, then register it here

            if (!services.IsNethereumAdaptersBound())
            {
                services.UseNethereumAdapters();
            }

            return services;
        }

        public static MudFacade Mud(this Web3.Web3 web3) => web3.ServiceProvider.GetRequiredService<MudFacade>();
    }
}