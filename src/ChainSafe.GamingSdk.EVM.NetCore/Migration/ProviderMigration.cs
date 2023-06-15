using System.Threading.Tasks;
using ChainSafe.GamingWeb3.Build;
using Web3Unity.Scripts.Library.Ethers.Migration;
using Web3Unity.Scripts.Library.Ethers.NetCore;
using Web3Unity.Scripts.Library.Ethers.Signers;

namespace Web3Unity.Scripts.Library.Ethers.Providers
{
    public static class ProviderMigration
    {
        public static ValueTask<JsonRpcProvider> NewJsonRpcProviderAsync(string url = "", Network.Network network = null) =>
            MigrationHelper.NewJsonRpcProviderAsync(url, network, BindEnvironment);

        private static void BindEnvironment(IWeb3ServiceCollection services)
        {
            services.UseNetCoreEnvironment();
        }
    }
}