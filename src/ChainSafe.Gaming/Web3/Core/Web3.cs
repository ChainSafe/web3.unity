#nullable enable
using System;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Contracts;
using ChainSafe.Gaming.Evm.Providers;
using ChainSafe.Gaming.Evm.Signers;
using ChainSafe.Gaming.Web3.Core;
using ChainSafe.Gaming.Web3.Core.Evm;
using Microsoft.Extensions.DependencyInjection;

namespace ChainSafe.Gaming.Web3
{
    /// <summary>
    /// Facade for all Web3-related services.
    /// Use this as an entry point to all SDK features.
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

        /// <summary>
        /// Access the <see cref="IRpcProvider"/> component, which provides RPC communication with the Ethereum network.
        /// </summary>
        public IRpcProvider RpcProvider => AssertComponentAccessible(rpcProvider, nameof(RpcProvider));

        /// <summary>
        /// Access the <see cref="ISigner"/> component, responsible for signing transactions, messages, and providing the player's public address.
        /// </summary>
        public ISigner Signer => AssertComponentAccessible(signer, nameof(Signer));

        /// <summary>
        /// Access the <see cref="ITransactionExecutor"/> component, used for sending transactions to the blockchain.
        /// </summary>
        public ITransactionExecutor TransactionExecutor => AssertComponentAccessible(transactionExecutor, nameof(TransactionExecutor));

        /// <summary>
        /// Access the event service of the Web3 instance, allowing you to subscribe to blockchain events.
        /// </summary>
        public IEvmEvents Events => AssertComponentAccessible(events, nameof(Events));

        /// <summary>
        /// Access the factory for creating Ethereum smart contract wrappers.
        /// </summary>
        public IContractBuilder ContractBuilder { get; }

        /// <summary>
        /// Access the project configuration object, providing access to project-specific settings.
        /// </summary>
        public IProjectConfig ProjectConfig { get; }

        /// <summary>
        /// Access the chain configuration object, providing access to blockchain-specific settings.
        /// </summary>
        public IChainConfig ChainConfig { get; }

        /// <summary>
        /// Access the service provider of this Web3 instance.
        /// Use this to retrieve any service that was bound to this Web3 instance during the build phase.
        /// </summary>
        public IServiceProvider ServiceProvider => serviceProvider;

        async ValueTask IAsyncDisposable.DisposeAsync()
        {
            await TerminateAsync();
            GC.SuppressFinalize(this);
        }

        internal async ValueTask InitializeAsync()
        {
            foreach (var lifecycleParticipant in serviceProvider.GetServices<ILifecycleParticipant>())
            {
                await lifecycleParticipant.WillStartAsync();
            }

            initialized = true;
        }

        /// <summary>
        /// Terminate this Web3 instance together with all of its components.
        /// </summary>
        /// <exception cref="Web3Exception">Web3 was already terminated.</exception>
        /// <returns>Task handle for the asynchronous process.</returns>
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

        private T AssertComponentAccessible<T>(T? value, string propertyName)
            where T : notnull
        {
            if (value == null)
            {
                throw new Web3Exception(
                    $"{propertyName} is not bound. Make sure to add an implementation of {propertyName} before using it.");
            }

            // TODO: uncomment after migration complete
            if (!initialized)
            {
                throw new Web3Exception($"Can't access {propertyName}. Web3 instance should be initialized first.");
            }

            return value;
        }
    }
}