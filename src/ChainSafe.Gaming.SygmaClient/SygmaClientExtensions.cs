using ChainSafe.Gaming.SygmaClient.Types;
using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Core;

namespace ChainSafe.Gaming.SygmaClient
{
    public static class SygmaClientExtensions
    {
        public static IWeb3ServiceCollection UseSygmaClient(this IWeb3ServiceCollection services)
        {
            services.AddSingleton<ISygmaClient, ILifecycleParticipant, SygmaClient>();
            return services;
        }
    }
}