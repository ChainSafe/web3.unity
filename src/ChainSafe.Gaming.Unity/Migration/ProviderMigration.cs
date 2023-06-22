using System.Threading.Tasks;
using ChainSafe.Gaming.Build;
using ChainSafe.Gaming.Evm;
using ChainSafe.Gaming.Evm.JsonRpcProvider;
using ChainSafe.Gaming.Migration;
using ChainSafe.Gaming.Unity.Environment;

namespace ChainSafe.Gaming.Unity.Migration
{
    public static class ProviderMigration
    {
        public static ValueTask<JsonRpcProvider> NewJsonRpcProviderAsync(string url = "", Network network = null)
            => MigrationHelper.NewJsonRpcProviderAsync(url, network, BindEnvironment);

        private static void BindEnvironment(IWeb3ServiceCollection services) =>
            services.UseUnityEnvironment();
    }
}