using ChainSafe.GamingWeb3.Build;
using Web3Unity.Scripts.Library.Ethers.Migration;
using Web3Unity.Scripts.Library.Ethers.Providers;
using Web3Unity.Scripts.Library.Ethers.Signers;
using ChainSafe.GamingWeb3.Unity;
using Web3Unity.Scripts.Library.Ethers.Network;

namespace Web3Unity.Scripts.Library.Ethers.Providers
{
    public static class ProviderMigration
    {
        public static JsonRpcProvider NewJsonRpcProvider(string url = "", Network.Network network = null)
        {
            return MigrationHelper.NewJsonRpcProvider(url, network, BindEnvironment);
        }

        public static JsonRpcSigner GetSigner(this JsonRpcProvider provider, int index = 0)
        {
            return MigrationHelper.NewJsonRpcSigner(provider, index, BindEnvironment);
        }

        private static void BindEnvironment(IWeb3ServiceCollection services) =>
            services
                .ConfigureUnityEnvironment(new UnityEnvironmentConfiguration
                {
                    DataDog = new ChainSafe.GamingSdk.Evm.Unity.DataDogAnalyticsConfiguration
                    {
                        ApiKey = "",
                    }
                })
                .UseUnityEnvironment();
    }
}