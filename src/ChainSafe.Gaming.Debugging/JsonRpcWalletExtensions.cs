using ChainSafe.Gaming.Evm.Signers;
using ChainSafe.Gaming.Wallets;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Core;
using ChainSafe.Gaming.Web3.Core.Evm;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

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