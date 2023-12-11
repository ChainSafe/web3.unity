using ChainSafe.Gaming.Web3.Build;
using Microsoft.Extensions.DependencyInjection;

namespace ChainSafe.Gaming.MetaMask.Unity
{
    /// <summary>
    /// <see cref="MetaMaskProvider"/> extension methods.
    /// </summary>
    public static class MetaMaskProviderExtensions
    {
        /// <summary>
        /// Binds implementation of <see cref="IMetaMaskProvider"/> as <see cref="MetaMaskProvider"/> to Web3 as a service.
        /// </summary>
        /// <param name="collection">Service collection to bind implementations to.</param>
        /// <returns>The same service collection that was passed in. This enables fluent style.</returns>
        public static IWeb3ServiceCollection UseMetaMask(this IWeb3ServiceCollection collection)
        {
            collection.AssertServiceNotBound<IMetaMaskProvider>();

            // wallet
            collection.AddSingleton<IMetaMaskProvider, MetaMaskProvider>();

            return collection;
        }
    }
}