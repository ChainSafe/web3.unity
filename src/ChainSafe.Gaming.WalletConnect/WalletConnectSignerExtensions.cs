using ChainSafe.Gaming.Evm.Signers;
using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Core;

namespace ChainSafe.Gaming.WalletConnect
{
    /// <summary>
    /// <see cref="WalletConnectSigner"/> extension methods.
    /// </summary>
    public static class WalletConnectSignerExtensions
    {
        /// <summary>
        /// Binds implementation of <see cref="ISigner"/> as <see cref="WalletConnectSigner"/> to Web3 as a service.
        /// </summary>
        /// <param name="collection">Service collection to bind implementations to.</param>
        /// <returns>The same service collection that was passed in. This enables fluent style.</returns>
        public static IWeb3ServiceCollection UseWalletConnectSigner(this IWeb3ServiceCollection collection)
        {
            collection.AssertServiceNotBound<ISigner>();

            // wallet
            collection.AddSingleton<ISigner, ILifecycleParticipant, WalletConnectSigner>();

            return collection;
        }
    }
}