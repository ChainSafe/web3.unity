using ChainSafe.Gaming.LocalStorage;
using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Evm.Wallet;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ChainSafe.Gaming.HyperPlay
{
    public static class HyperPlayExtensions
    {
        /// <summary>
        /// Binds implementation of <see cref="IWalletProvider"/> as <see cref="HyperPlayProvider"/> to Web3 as a service.
        /// </summary>
        /// <param name="collection">Service collection to bind implementations to.</param>
        /// <param name="config">Config for connecting via HyperPlay.</param>
        /// <returns>The same service collection that was passed in. This enables fluent style.</returns>
        public static IWeb3ServiceCollection UseHyperPlay(this IWeb3ServiceCollection collection, IHyperPlayConfig config)
        {
            return collection.UseHyperPlay<HyperPlayProvider>(config);
        }

        /// <summary>
        /// Binds implementation of <see cref="IWalletProvider"/> as <see cref="T"/> to Web3 as a service.
        /// </summary>
        /// <param name="collection">Service collection to bind implementations to.</param>
        /// <param name="config">Config for connecting via HyperPlay.</param>
        /// <typeparam name="T">Type of <see cref="HyperPlayProvider"/>.</typeparam>
        /// <returns>The same service collection that was passed in. This enables fluent style.</returns>
        public static IWeb3ServiceCollection UseHyperPlay<T>(this IWeb3ServiceCollection collection, IHyperPlayConfig config)
            where T : HyperPlayProvider
        {
            collection.AssertServiceNotBound<IHyperPlayData>();

            collection.AddSingleton<IHyperPlayData, IStorable, HyperPlayData>();

            collection.AssertServiceNotBound<IHyperPlayConfig>();

            collection.Replace(ServiceDescriptor.Singleton(typeof(IHyperPlayConfig), config));

            collection.UseWalletProvider<T>(config);

            return collection;
        }
    }
}