using ChainSafe.Gaming.Evm.Signers;
using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ChainSafe.Gaming.Web3.Evm.JsonRpc
{
    public static class SignerExtension
    {
        /// <summary>
        /// DEBUG ONLY! Use JsonRpc signer that uses `eth_account` RPC calls
        /// to interact with private keys stored on Node.
        /// </summary>
        /// <param name="collection">Collection of Web3Services.</param>
        /// <param name="config">Configuration of JsonRpcSigner.</param>;
        /// <returns>Updated Collection of Web3Services.</returns>
        public static IWeb3ServiceCollection UseJsonRpcSigner(this IWeb3ServiceCollection collection, SignerConfig config)
        {
            collection.AssertServiceNotBound<ISigner>();
            collection.Replace(ServiceDescriptor.Singleton(config));
            collection.AddSingleton<ISigner, ILifecycleParticipant, Signer>();
            return collection;
        }
    }
}