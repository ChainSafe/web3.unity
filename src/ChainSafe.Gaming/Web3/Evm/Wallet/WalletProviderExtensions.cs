using ChainSafe.Gaming.Evm.Signers;
using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ChainSafe.Gaming.Web3.Evm.Wallet
{
    public static class WalletProviderExtensions
    {
        public static IWeb3ServiceCollection UseWalletProvider<TProvider>(this IWeb3ServiceCollection collection, IWalletProviderConfig config)
            where TProvider : WalletProvider
        {
            collection.AssertServiceNotBound<IWalletProvider>();

            collection.AddSingleton<IWalletProvider, TProvider>();

            collection.Replace(ServiceDescriptor.Singleton(typeof(IWalletProviderConfig), config));

            return collection;
        }

        public static IWeb3ServiceCollection UseWalletSigner(this IWeb3ServiceCollection collection)
        {
            collection.AssertServiceNotBound<ISigner>();

            collection.AddSingleton<ISigner, ILifecycleParticipant, WalletSigner>();

            return collection;
        }
    }
}