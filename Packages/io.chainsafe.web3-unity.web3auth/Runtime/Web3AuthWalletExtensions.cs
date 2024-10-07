using ChainSafe.Gaming.Evm.Signers;
using ChainSafe.Gaming.InProcessSigner;
using ChainSafe.Gaming.LocalStorage;
using ChainSafe.Gaming.UnityPackage;
using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Core;
using ChainSafe.Gaming.Web3.Core.Evm;
using ChainSafe.Gaming.Web3.Core.Logout;
using ChainSafe.Gaming.Web3.Evm.Wallet;
using ChainSafe.GamingSdk.Web3Auth;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

/// <summary>
/// A static class providing extension methods to configure and use a Web3AuthWallet within an IWeb3ServiceCollection.
/// </summary>
public static class Web3AuthWalletExtensions
{
    /// <summary>
    /// Configures and registers a Web3AuthWallet within an IWeb3ServiceCollection with the provided configuration.
    /// </summary>
    /// <param name="collection">The IWeb3ServiceCollection to configure the Web3AuthWallet within.</param>
    /// <param name="configuration">The configuration for the Web3AuthWallet.</param>
    /// <returns>The modified IWeb3ServiceCollection with the Web3AuthWallet configuration.</returns>
    public static IWeb3ServiceCollection UseWeb3AuthWallet(this IWeb3ServiceCollection collection, Web3AuthWalletConfig configuration)
    {
        collection.UseWeb3AuthWallet();
        collection.ConfigureWeb3AuthWallet(configuration);
        return collection;
    }

    /// <summary>
    /// Registers and configures a Web3AuthWallet within an IWeb3ServiceCollection.
    /// </summary>
    /// <param name="collection">The IWeb3ServiceCollection to register the Web3AuthWallet within.</param>
    /// <returns>The modified IWeb3ServiceCollection with the Web3AuthWallet registered.</returns>
    public static IWeb3ServiceCollection UseWeb3AuthWallet(this IWeb3ServiceCollection collection)
    {
        collection.AssertServiceNotBound<ISigner>();
        collection.AssertServiceNotBound<ITransactionExecutor>();

#if UNITY_WEBGL && !UNITY_EDITOR
        collection.Replace(ServiceDescriptor.Singleton<ILocalStorage, WebDataStorage>());
        
        collection.AddSingleton<IWalletProvider, IAccountProvider, Web3AuthWebGLProvider>();
#else
        collection.AddSingleton<IWalletProvider, IAccountProvider, Web3AuthProvider>();
#endif

        collection.AddSingleton<ISigner, ILifecycleParticipant, ILogoutHandler, Web3AuthSigner>();

        collection.AddSingleton<ITransactionExecutor, IWeb3AuthTransactionHandler, Web3AuthTransactionExecutor>();

        return collection;
    }

    /// <summary>
    /// Replaces the existing Web3AuthWallet configuration within an IWeb3ServiceCollection with the provided configuration.
    /// </summary>
    /// <param name="collection">The IWeb3ServiceCollection to configure the Web3AuthWallet within.</param>
    /// <param name="configuration">The configuration for the Web3AuthWallet.</param>
    /// <returns>The modified IWeb3ServiceCollection with the Web3AuthWallet configuration replaced.</returns>
    public static IWeb3ServiceCollection ConfigureWeb3AuthWallet(this IWeb3ServiceCollection collection, Web3AuthWalletConfig configuration)
    {
        collection.Replace(ServiceDescriptor.Singleton(typeof(Web3AuthWalletConfig), configuration));
        return collection;
    }
}
