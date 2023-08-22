using ChainSafe.GamingSdk.Web3Auth;
using ChainSafe.GamingSDK.EVM.Web3.Core;
using ChainSafe.GamingSDK.EVM.Web3.Core.Evm;
using ChainSafe.GamingWeb3.Build;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Web3Unity.Scripts.Library.Ethers.Signers;

public static class Web3AuthWalletExtensions
{
    public static IWeb3ServiceCollection UseWeb3AuthWallet(this IWeb3ServiceCollection collection, Web3AuthWalletConfig configuration)
    {
        collection.UseWeb3AuthWallet();
        collection.ConfigureWeb3AuthWallet(configuration);
        return collection;
    }

    public static IWeb3ServiceCollection UseWeb3AuthWallet(this IWeb3ServiceCollection collection)
    {
        collection.AssertServiceNotBound<ISigner>();
        collection.AssertServiceNotBound<ITransactionExecutor>();

        collection.AddSingleton<ISigner, ITransactionExecutor, ILifecycleParticipant, Web3AuthWallet>();

        return collection;
    }

    public static IWeb3ServiceCollection ConfigureWeb3AuthWallet(this IWeb3ServiceCollection collection, Web3AuthWalletConfig configuration)
    {
        collection.Replace(ServiceDescriptor.Singleton(typeof(Web3AuthWalletConfig), configuration));
        return collection;
    }
}
