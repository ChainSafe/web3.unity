#nullable enable
using System;
using System.Threading.Tasks;
using ChainSafe.GamingSDK.EVM.Web3.Core;
using ChainSafe.GamingSDK.EVM.Web3.Core.Evm;
using Microsoft.Extensions.DependencyInjection;
using Web3Unity.Scripts.Library.Ethers.Contracts;
using Web3Unity.Scripts.Library.Ethers.Providers;
using Web3Unity.Scripts.Library.Ethers.Signers;

namespace ChainSafe.GamingWeb3
{
    /// <summary>
    /// Facade for all Web3-related services.
    /// </summary>
    public class Web3 : IAsyncDisposable
    {
        private readonly ServiceProvider serviceProvider;
        private readonly IRpcProvider? rpcProvider;
        private readonly ISigner? signer;
        private readonly ITransactionExecutor? transactionExecutor;
        private readonly IEvmEvents? events;

        private bool initialized;
        private bool terminated;

        internal Web3(ServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            rpcProvider = serviceProvider.GetService<IRpcProvider>();
            signer = serviceProvider.GetService<ISigner>();
            transactionExecutor = serviceProvider.GetService<ITransactionExecutor>();
            events = serviceProvider.GetRequiredService<IEvmEvents>();
            ContractBuilder = serviceProvider.GetRequiredService<IContractBuilder>();
            ProjectConfig = serviceProvider.GetRequiredService<IProjectConfig>();
            ChainConfig = serviceProvider.GetRequiredService<IChainConfig>();
        }

        public IRpcProvider RpcProvider => AssertComponentAccessible(rpcProvider, nameof(RpcProvider));

        public ISigner Signer => AssertComponentAccessible(signer, nameof(Signer));

        public ITransactionExecutor TransactionExecutor => AssertComponentAccessible(transactionExecutor, nameof(TransactionExecutor));

        public IEvmEvents Events => AssertComponentAccessible(events, nameof(Events));

        public IContractBuilder ContractBuilder { get; }

        public IProjectConfig ProjectConfig { get; }

        public IChainConfig ChainConfig { get; }

        public IServiceProvider ServiceProvider => serviceProvider;

        private static T AssertComponentAccessible<T>(T? value, string propertyName)
            where T : notnull
        {
            if (value == null)
            {
                throw new Web3Exception(
                  $"{propertyName} is not bound. Make sure to add an implementation of {propertyName} before using it.");
            }

            // todo uncomment after migration complete
            // if (!_initialized)
            // {
            //   throw new Web3Exception($"Can't access {propertyName}. Initialize Web3 first.");
            // }
            return value;
        }

        async ValueTask IAsyncDisposable.DisposeAsync()
        {
            await TerminateAsync();
            GC.SuppressFinalize(this);
        }

        public async ValueTask InitializeAsync()
        {
            if (initialized)
            {
                throw new Web3Exception("Web3 was already initialized.");
            }

            foreach (var lifecycleParticipant in serviceProvider.GetServices<ILifecycleParticipant>())
            {
                await lifecycleParticipant.WillStartAsync();
            }

            // todo initialize other components
            initialized = true;
        }

        public async ValueTask TerminateAsync()
        {
            if (terminated)
            {
                throw new Web3Exception("Web3 was already terminated.");
            }

            if (initialized)
            {
                foreach (var lifecycleParticipant in serviceProvider.GetServices<ILifecycleParticipant>())
                {
                    await lifecycleParticipant.WillStopAsync();
                }
            }

            serviceProvider.Dispose();
            terminated = true;
        }
    }
}