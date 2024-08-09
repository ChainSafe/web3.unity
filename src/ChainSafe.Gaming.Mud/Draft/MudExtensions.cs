using ChainSafe.Gaming.Mud.Draft.InMemory;
using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Core;
using ChainSafe.Gaming.Web3.Core.Nethereum;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ChainSafe.Gaming.Mud.Draft
{
    public static class MudExtensions
    {
        public static IWeb3ServiceCollection UseDraftMud(this IWeb3ServiceCollection services, MudConfig mudConfig)
        {
            services.AssertServiceNotBound<MudFacade>();

            services.Replace(ServiceDescriptor.Singleton<IMudConfig>(mudConfig));

            services.AddSingleton<MudFacade>();
            services.AddSingleton<MudWorldFactory>();
            services.AddSingleton<IMudStorageFactory, MudStorageFactory>();

            // Storage strategies
            services.AddSingleton<InMemoryMudStorage>();

            if (!services.IsNethereumAdaptersBound())
            {
                services.UseNethereumAdapters();
            }

            return services;
        }

        public static MudFacade DraftMud(this Web3.Web3 web3) => web3.ServiceProvider.GetRequiredService<MudFacade>();
    }
}