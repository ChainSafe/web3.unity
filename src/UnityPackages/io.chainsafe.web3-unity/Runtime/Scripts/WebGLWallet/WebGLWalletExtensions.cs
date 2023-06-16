using ChainSafe.GamingSDK.EVM.Web3.Core;
using ChainSafe.GamingSDK.EVM.Web3.Core.Evm;
using ChainSafe.GamingWeb3.Build;
using Web3Unity.Scripts.Library.Ethers.Signers;

namespace ChainSafe.GamingSDK.EVM.WebGLWallet
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