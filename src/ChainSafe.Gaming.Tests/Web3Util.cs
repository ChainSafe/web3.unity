using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Contracts.Builders;
using ChainSafe.Gaming.Evm.JsonRpc;
using ChainSafe.Gaming.Evm.Providers;
using ChainSafe.Gaming.Evm.Signers;
using ChainSafe.Gaming.NetCore;
using ChainSafe.Gaming.Wallets;
using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Core;
using ChainSafe.Gaming.Web3.Core.Evm;
using Microsoft.Extensions.DependencyInjection;

namespace ChainSafe.Gaming.Tests
{
    internal static class Web3Util
    {
        public static ValueTask<ChainSafe.Gaming.Web3.Web3> CreateWeb3(int accountIndex = 0)
        {
            return CreateWeb3(new JsonRpcWalletConfiguration { AccountIndex = accountIndex });
        }

        private static ValueTask<ChainSafe.Gaming.Web3.Web3> CreateWeb3(
            JsonRpcWalletConfiguration jsonRpcWalletConfiguration)
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

        public static string DeployContracts(ChainSafe.Gaming.Web3.Web3 web3, string bytecode, string abi)
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