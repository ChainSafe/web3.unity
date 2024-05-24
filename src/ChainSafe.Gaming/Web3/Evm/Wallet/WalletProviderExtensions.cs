using ChainSafe.Gaming.Evm.Signers;
using ChainSafe.Gaming.Web3.Build;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ChainSafe.Gaming.Web3.Evm.Wallet
{
    public static class WalletProviderExtensions
    {
        internal static IWeb3ServiceCollection UseWalletProvider<TProvider>(this IWeb3ServiceCollection collection, IWalletConfig config)
            where TProvider : WalletProvider
        {
            collection.AssertServiceNotBound<IWalletProvider>();

            collection.AddSingleton<IWalletProvider, TProvider>();

            collection.Replace(ServiceDescriptor.Singleton(typeof(IWalletConfig), config));

            return collection;
        }

        internal static IWeb3ServiceCollection UseWalletSigner<TSigner>(this IWeb3ServiceCollection collection)
            where TSigner : WalletSigner
        {
            collection.AssertServiceNotBound<ISigner>();

            collection.AddSingleton<ISigner, TSigner>();

            return collection;
        }
    }
}