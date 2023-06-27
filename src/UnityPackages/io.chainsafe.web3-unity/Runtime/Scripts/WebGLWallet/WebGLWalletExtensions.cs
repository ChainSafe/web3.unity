using ChainSafe.Gaming.Build;
using ChainSafe.Gaming.Evm;
using ChainSafe.Gaming.Lifecycle;

namespace ChainSafe.Gaming.Unity.Wallets.WebGLWallet
{
    public static class WebGLWalletExtensions
    {
        public static IWeb3ServiceCollection UseWebGLWallet(this IWeb3ServiceCollection collection)
        {
            collection.AssertServiceNotBound<ISigner>();
            collection.AssertServiceNotBound<ITransactionExecutor>();

            collection.AddSingleton<ISigner, ITransactionExecutor, ILifecycleParticipant, WebGLWallet>();

            return collection;
        }
    }
}