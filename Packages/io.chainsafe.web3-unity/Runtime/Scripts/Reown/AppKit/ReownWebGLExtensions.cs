#if UNITY_WEBGL && !UNITY_EDITOR
using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Core;
using ChainSafe.Gaming.Web3.Evm.Wallet;
using Microsoft.Extensions.DependencyInjection;

namespace ChainSafe.Gaming.Reown.AppKit
{
    public static class ReownWebGLExtensions
    {
        public static IWeb3ServiceCollection UseReownWebGL(
            this IWeb3ServiceCollection services,
            IReownConfig config)
        {
            services.AssertServiceNotBound<IWalletProvider>();
            services.ConfigureReown(config);
            services.AddSingleton<IConnectionHelper, ILifecycleParticipant, ReownWebGLProvider>();
            services.AddSingleton<ReownHttpClient>();
            services.UseWalletProvider<ReownWebGLProvider>(config);
            return services;
        }


    }
}
#endif