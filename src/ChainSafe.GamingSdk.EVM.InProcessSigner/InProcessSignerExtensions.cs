using ChainSafe.GamingWeb3.Build;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Web3Unity.Scripts.Library.Ethers.Signers;

namespace ChainSafe.GamingSdk.EVM.InProcessSigner
{
    public static class InProcessSignerExtensions
    {
        public static IWeb3ServiceCollection UseInProcessSigner(this IWeb3ServiceCollection collection, InProcessSignerConfig configuration)
        {
            collection.UseInProcessSigner();
            collection.ConfigureInProcessSigner(configuration);
            return collection;
        }

        public static IWeb3ServiceCollection UseInProcessSigner(this IWeb3ServiceCollection collection)
        {
            collection.AssertServiceNotBound<ISigner>();
            collection.AddSingleton<ISigner, InProcessSigner>();
            return collection;
        }

        public static IWeb3ServiceCollection ConfigureInProcessSigner(this IWeb3ServiceCollection collection, InProcessSignerConfig configuration)
        {
            collection.Replace(ServiceDescriptor.Singleton(typeof(InProcessSignerConfig), configuration));
            return collection;
        }
    }
}
