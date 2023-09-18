
using ChainSafe.Gaming.Evm.Signers;
using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Core;
using ChainSafe.Gaming.Web3.Core.Evm;

namespace ChainSafe.Gaming.Wallets
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