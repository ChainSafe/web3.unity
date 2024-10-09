using System.Collections.Generic;
using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Core;
using ChainSafe.Gaming.Web3.Core.Chains;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ChainSafe.Gaming.MultiCall
{
    public static class MultiCallExtensions
    {
        private static readonly MultiCallConfig DefaultConfig = new(new Dictionary<string, string>());

        public static IMultiCall MultiCall(this Web3.Web3 web3) => web3.ServiceProvider.GetRequiredService<IMultiCall>();

        /// <summary>
        /// Binds implementation of MultiCall to Web3.
        /// </summary>
        /// <returns>The same service collection that was passed in. This enables fluent style.</returns>
        public static IWeb3ServiceCollection UseMultiCall(this IWeb3ServiceCollection collection, MultiCallConfig configuration = null)
        {
            collection.AssertServiceNotBound<IMultiCall>();

            collection.TryAddSingleton(configuration ?? DefaultConfig);

            collection.AddSingleton<IMultiCall, ILifecycleParticipant, IChainSwitchHandler, MultiCall>();
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