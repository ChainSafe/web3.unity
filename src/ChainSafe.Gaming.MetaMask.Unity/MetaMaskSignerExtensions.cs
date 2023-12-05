using ChainSafe.Gaming.Evm.Signers;
using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Core;

namespace ChainSafe.Gaming.MetaMask.Unity
{
    public static class MetaMaskSignerExtensions
    {
        public static IWeb3ServiceCollection UseMetaMaskSigner(this IWeb3ServiceCollection collection)
        {
            collection.AssertServiceNotBound<ISigner>();

            // wallet
            collection.AddSingleton<ISigner, ILifecycleParticipant, MetaMaskSigner>();

            return collection;
        }
    }
}