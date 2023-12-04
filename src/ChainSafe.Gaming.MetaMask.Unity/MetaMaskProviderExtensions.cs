using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Core;

namespace ChainSafe.Gaming.MetaMask.Unity
{
    public static class MetaMaskProviderExtensions
    {
        public static IWeb3ServiceCollection UseMetaMask(this IWeb3ServiceCollection collection)
        {
            collection.AssertServiceNotBound<IMetaMaskProvider>();

            // wallet
            collection.AddSingleton<IMetaMaskProvider, ILifecycleParticipant, MetaMaskProvider>();

            return collection;
        }
    }
}