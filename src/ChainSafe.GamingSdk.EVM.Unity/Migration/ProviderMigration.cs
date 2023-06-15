﻿using ChainSafe.GamingWeb3.Build;
using ChainSafe.GamingWeb3.Unity;
using Web3Unity.Scripts.Library.Ethers.Migration;
using Web3Unity.Scripts.Library.Ethers.Signers;

namespace Web3Unity.Scripts.Library.Ethers.Providers
{
    public static class ProviderMigration
    {
        public static JsonRpcProvider NewJsonRpcProvider(string url = "", Network.Network network = null)
        {
            return MigrationHelper.NewJsonRpcProvider(url, network, BindEnvironment);
        }

        private static void BindEnvironment(IWeb3ServiceCollection services) =>
            services
                .ConfigureUnityEnvironment(new UnityEnvironmentConfiguration())
                .UseUnityEnvironment();
    }
}