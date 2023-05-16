using ChainSafe.GamingWeb3.Build;
using ChainSafe.GamingWeb3.Unity;
using Web3Unity.Scripts.Library.Ethers.Migration;
using Web3Unity.Scripts.Library.Ethers.Providers;

namespace Web3Unity.Scripts.Library.Ethers.Signers
{
    public static class SignerMigration
    {
        public static JsonRpcSigner NewJsonRpcSigner(JsonRpcProvider provider, string address)
        {
            return MigrationHelper.NewJsonRpcSigner(provider, address, BindEnvironment);
        }

        public static JsonRpcSigner NewJsonRpcSigner(JsonRpcProvider provider, int index)
        {
            return MigrationHelper.NewJsonRpcSigner(provider, index, BindEnvironment);
        }

        private static void BindEnvironment(IWeb3ServiceCollection services) =>
            services
                .ConfigureUnityEnvironment(new UnityEnvironmentConfiguration
                {
                    DataDog = new ChainSafe.GamingSdk.Evm.Unity.DataDogAnalyticsConfiguration
                    {
                        ApiKey = string.Empty,
                    },
                })
                .UseUnityEnvironment();
    }
}