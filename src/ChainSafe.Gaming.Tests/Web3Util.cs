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
    /// <summary>
    /// A utility class for working with Web3 instances and deploying smart contracts.
    /// </summary>
    internal static class Web3Util
    {
        /// <summary>
        /// Creates a new Web3 instance using default settings and an optional account index.
        /// </summary>
        /// <param name="accountIndex">An optional account index.</param>
        /// <returns>A ValueTask of Web3.Web3.</returns>
        public static ValueTask<Web3.Web3> CreateWeb3(int accountIndex = 0)
        {
            return CreateWeb3(new JsonRpcWalletConfig { AccountIndex = accountIndex });
        }

        /// <summary>
        /// Creates a new Web3 instance with custom configurations and an optional account index.
        /// </summary>
        /// <param name="configureDelegate">A delegate to configure services.</param>
        /// <param name="accountIndex">An optional account index.</param>
        /// <returns>A ValueTask of Web3.Web3.</returns>
        public static ValueTask<Web3.Web3> CreateWeb3(Web3Builder.ConfigureServicesDelegate configureDelegate, int accountIndex = 0)
        {
            return CreateWeb3(new JsonRpcWalletConfig { AccountIndex = accountIndex }, configureDelegate);
        }

        /// <summary>
        /// Creates a new Web3 instance with custom configurations.
        /// </summary>
        /// <param name="jsonRpcWalletConfig">The JSON-RPC wallet configuration.</param>
        /// <param name="configureDelegate">A delegate to configure services.</param>
        /// <returns>A ValueTask of Web3.Web3.</returns>
        private static ValueTask<Web3.Web3> CreateWeb3(
            JsonRpcWalletConfig jsonRpcWalletConfig, Web3Builder.ConfigureServicesDelegate configureDelegate = null)
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

                    services.AddSingleton(jsonRpcWalletConfig);
                    services.AddSingleton<ISigner, ITransactionExecutor, ILifecycleParticipant, JsonRpcWallet>();
                })
                .Configure(configureDelegate)
                .BuildAsync();
        }

        /// <summary>
        /// Deploys smart contracts using the provided Web3 instance, bytecode, and ABI.
        /// </summary>
        /// <param name="web3">The Web3 instance.</param>
        /// <param name="bytecode">The bytecode of the smart contract.</param>
        /// <param name="abi">The ABI (Application Binary Interface) of the smart contract.</param>
        /// <returns>The contract address of the deployed smart contract.</returns>
        public static string DeployContracts(Web3.Web3 web3, string bytecode, string abi)
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