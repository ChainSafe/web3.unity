using ChainSafe.GamingSDK.EVM.Web3AuthWallet;
using ChainSafe.GamingWeb3.Build;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Scripts.Web3AuthWallet
{
    public static class Web3AuthWalletExtensions
    {
        private static readonly Web3AuthWalletConfig DefaultConfig = new();

        /// <summary>
        /// Binds Web implementation of EVM Provider to Web3.
        /// </summary>
        /// <returns>The same service collection that was passed in. This enables fluent style.</returns>
        public static IWeb3ServiceCollection UseWeb3AuthWallet(
            this IWeb3ServiceCollection collection,
            Web3AuthWalletConfig configuration)
        {
            collection.UseWeb3AuthWallet(configuration);
            collection.ConfigureWeb3AuthWallet(configuration);
            return collection;
        }

        /// <summary>
        /// Binds Web implementation of EVM Provider to Web3.
        /// </summary>
        /// <returns>The same service collection that was passed in. This enables fluent style.</returns>
        public static IWeb3ServiceCollection UseWeb3AuthWallet(this IWeb3ServiceCollection collection)
        {
            //collection.AssertServiceNotBound<ISigner>();
            //collection.AssertServiceNotBound<ITransactionExecutor>();

            // config
            collection.TryAddSingleton(DefaultConfig);

            // wallet
            //collection.AddSingleton<ISigner>();

            return collection;
        }

        /// <summary>
        /// Configures Web implementation of EVM Provider.
        /// </summary>
        /// <returns>The same service collection that was passed in. This enables fluent style.</returns>
        public static IWeb3ServiceCollection ConfigureWeb3AuthWallet(
            this IWeb3ServiceCollection collection,
            Web3AuthWalletConfig configuration)
        {
            collection.Replace(ServiceDescriptor.Singleton(typeof(Web3AuthWalletConfig), configuration));
            return collection;
        }
    }
}