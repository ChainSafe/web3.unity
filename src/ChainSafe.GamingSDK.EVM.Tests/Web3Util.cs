using System.Threading.Tasks;
using ChainSafe.GamingSDK.EVM.Web3.Core;
using ChainSafe.GamingSDK.EVM.Web3.Core.Evm;
using ChainSafe.GamingWeb3.Build;
using Microsoft.Extensions.DependencyInjection;
using Web3Unity.Scripts.Library.Ethers.JsonRpc;
using Web3Unity.Scripts.Library.Ethers.NetCore;
using Web3Unity.Scripts.Library.Ethers.Providers;
using Web3Unity.Scripts.Library.Ethers.Signers;

namespace ChainSafe.GamingSDK.EVM.Tests
{
    using Web3 = ChainSafe.GamingWeb3.Web3;

    internal static class Web3Util
    {
        public static ValueTask<Web3> CreateWeb3() => CreateWeb3(0);

        public static ValueTask<Web3> CreateWeb3(int accountIndex) => CreateWeb3(new JsonRpcWalletConfiguration() { AccountIndex = accountIndex });

        public static ValueTask<Web3> CreateWeb3(JsonRpcWalletConfiguration jsonRpcWalletConfiguration)
        {
            return new Web3Builder(
                new ProjectConfig { ProjectId = string.Empty },
                new ChainConfig
                {
                    Chain = "Ganache",
                    ChainId = "88888888",
                    Network = "Geth Testnet",
                    Rpc = "http://127.0.0.1:7545",
                })
                .Configure(services =>
                {
                    services.UseNetCoreEnvironment();
                    services.UseJsonRpcProvider();

                    services.AddSingleton(jsonRpcWalletConfiguration);
                    services.AddSingleton<ISigner, ITransactionExecutor, ILifecycleParticipant, JsonRpcWallet>();
                })
                .BuildAsync();
        }
    }
}
