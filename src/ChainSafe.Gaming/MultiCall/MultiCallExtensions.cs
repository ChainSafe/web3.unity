using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ChainSafe.Gaming.MultiCall
{
    public static class MultiCallExtensions
    {
        private static readonly MultiCallConfig DefaultConfig = new MultiCallConfig(null);

        public static IMultiCall MultiCall(this Web3.Web3 web3) => web3.ServiceProvider.GetRequiredService<IMultiCall>();

        /// <summary>
        /// Binds implementation of MultiCall to Web3.
        /// </summary>
        /// <returns>The same service collection that was passed in. This enables fluent style.</returns>
        public static IWeb3ServiceCollection UseMultiCall(this IWeb3ServiceCollection collection, MultiCallConfig configuration)
        {
            collection.UseMultiCall();
            collection.ConfigureMultiCall(configuration);
            return collection;
        }

        /// <summary>
        /// Binds implementation of MultiCall to Web3.
        /// </summary>
        /// <returns>The same service collection that was passed in. This enables fluent style.</returns>
        public static IWeb3ServiceCollection UseMultiCall(this IWeb3ServiceCollection collection)
        {
            collection.AssertServiceNotBound<IMultiCall>();

            // config
            collection.TryAddSingleton(DefaultConfig);

            collection.AddSingleton<ILifecycleParticipant, IMultiCall, MultiCall>();
            return collection;
        }

        /// <summary>
        /// Configures MultiCall settings.
        /// </summary>
        /// <returns>The same service collection that was passed in. This enables fluent style.</returns>
        public static IWeb3ServiceCollection ConfigureMultiCall(this IWeb3ServiceCollection collection, MultiCallConfig configuration)
        {
            collection.Replace(ServiceDescriptor.Singleton(typeof(MultiCallConfig), configuration));

            return collection;
        }
    }
}