using ChainSafe.Gaming.Mud.Storages;
using ChainSafe.Gaming.Mud.Storages.InMemory;
using ChainSafe.Gaming.Mud.Worlds;
using ChainSafe.Gaming.RPC.Events;
using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Core.Nethereum;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ChainSafe.Gaming.Mud
{
    public static class MudExtensions
    {
        /// <summary>
        /// Configures and enables the use of MUD for the Web3 client that is being built.
        /// </summary>
        /// <param name="services">The Web3 service collection to configure.</param>
        /// <param name="mudConfig">The MUD configuration.</param>
        /// <returns>The same service collection that was passed in. This enables fluent style.</returns>
        public static IWeb3ServiceCollection UseMud(this IWeb3ServiceCollection services, IMudConfig mudConfig)
        {
            services.AssertServiceNotBound<MudFacade>();
            services.AssertConfigurationNotBound<IMudConfig>();

            services.ConfigureMud(mudConfig);
            services.UseMud();

            return services;
        }

        /// <summary>
        /// Configures the MUD services with the provided MUD configuration.
        /// </summary>
        /// <param name="services">The Web3 service collection to configure.</param>
        /// <param name="mudConfig">The MUD configuration.</param>
        /// <returns>The same service collection that was passed in. This enables fluent style.</returns>
        public static IWeb3ServiceCollection ConfigureMud(this IWeb3ServiceCollection services, IMudConfig mudConfig)
        {
            services.Replace(ServiceDescriptor.Singleton(mudConfig));

            return services;
        }

        /// <summary>
        /// Enables the use of MUD for the Web3 client that is being built.
        /// </summary>
        /// <param name="services">The Web3 service collection to configure.</param>
        /// <returns>The same service collection that was passed in. This enables fluent style.</returns>
        public static IWeb3ServiceCollection UseMud(this IWeb3ServiceCollection services)
        {
            services.AssertServiceNotBound<MudFacade>();

            services.AddSingleton<MudFacade>();
            services.AddSingleton<MudWorldFactory>();
            services.AddSingleton<IMudStorageFactory, MudStorageFactory>();

            // Storage strategies
            services.AddTransient<InMemoryMudStorage>(); // todo implement OffchainIndexerMudStorage, then register it in the next line

            if (!services.IsBound<INethereumWeb3Adapter>())
            {
                services.UseNethereumAdapters();
            }

            return services;
        }

        /// <summary>
        /// Retrieves the MudFacade instance, a facade class for all the MUD-related functionality, from the Web3 service provider.
        /// </summary>
        /// <param name="web3">The Web3 client.</param>
        /// <returns>The MudFacade instance.</returns>
        public static MudFacade Mud(this Web3.Web3 web3) => web3.ServiceProvider.GetRequiredService<MudFacade>();
    }
}