using System;
using System.Text.Json;
using System.Threading.Tasks;
using ChainSafe.GamingSDK.EVM.Web3.Core;
using ChainSafe.GamingSDK.EVM.Web3.Core.Evm;
using ChainSafe.GamingWeb3.Build;
using Microsoft.Extensions.DependencyInjection;
using Web3Unity.Scripts.Library.Ethers.Contracts.Builders;
using Web3Unity.Scripts.Library.Ethers.JsonRpc;
using Web3Unity.Scripts.Library.Ethers.NetCore;
using Web3Unity.Scripts.Library.Ethers.Providers;
using Web3Unity.Scripts.Library.Ethers.Signers;

namespace ChainSafe.GamingSDK.EVM.Tests
{
    using Web3 = ChainSafe.GamingWeb3.Web3;

    internal static class Web3Util
    {
        public static ValueTask<Web3> CreateWeb3(int accountIndex = 0) => CreateWeb3(new JsonRpcWalletConfiguration() { AccountIndex = accountIndex });

        private static ValueTask<Web3> CreateWeb3(JsonRpcWalletConfiguration jsonRpcWalletConfiguration)
        {
            return new Web3Builder(
                new ProjectConfig { ProjectId = string.Empty },
                new ChainConfig
                {
                    Chain = "Anvil",
                    ChainId = "31337",
                    Network = "GoChain Testnet",
                    Rpc = "http://127.0.0.1:8545",
                })
                .Configure(services =>
                {
                    services.UseNetCoreEnvironment();
                    services.UseRpcProvider();

                    services.AddSingleton(jsonRpcWalletConfiguration);
                    services.AddSingleton<ISigner, ITransactionExecutor, ILifecycleParticipant, JsonRpcWallet>();
                })
                .BuildAsync();
        }

        public static string DeployContracts(Web3 web3, string bytecode, string abi)
        {
            var builder = new DeployContractTransactionBuilder();
            var txReq = builder.BuildTransaction(bytecode, abi, null);

            var txTask = web3.TransactionExecutor.SendTransaction(txReq);
            txTask.Wait();

            var receiptTask = web3.RpcProvider.GetTransactionReceipt(txTask.Result.Hash);
            receiptTask.Wait();

            return receiptTask.Result.ContractAddress ?? string.Empty;
        }
    }
}
