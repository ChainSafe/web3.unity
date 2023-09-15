using ChainSafe.GamingSDK.EVM.Web3.Core;
using ChainSafe.GamingSDK.EVM.Web3.Core.Evm;
using ChainSafe.GamingWeb3.Build;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Web3Unity.Scripts.Library.Ethers.Signers;

namespace ChainSafe.Gaming.Debugging
{
    public static class JsonRpcWalletExtensions
    {
        public static DebugBuildSubCategory UseJsonRpcWallet(this DebugBuildSubCategory debugServices, JsonRpcWalletConfig configuration)
        {
            var buildSubCategory = (IWeb3BuildSubCategory)debugServices;
            var services = buildSubCategory.Services;

            services.AddSingleton<ISigner, ITransactionExecutor, ILifecycleParticipant, JsonRpcWallet>();
            services.Replace(ServiceDescriptor.Singleton(configuration));

            return debugServices;
        }
    }
}