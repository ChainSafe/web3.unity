using ChainSafe.GamingWeb3.Build;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Web3Unity.Scripts.Library.Ethers.Signers;

namespace ChainSafe.GamingSDK.EVM.Web3.Core.Evm.MultiCall
{
    public static class MultiCallExtensions
    {
        private static readonly MultiCallConfig DefaultConfig = new();

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
            collection.AssertServiceNotBound<ISigner>();
            collection.AssertServiceNotBound<ITransactionExecutor>();

            // config
            collection.TryAddSingleton(DefaultConfig);

            // wallet
            collection.AddSingleton<ILifecycleParticipant, MultiCall>();

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