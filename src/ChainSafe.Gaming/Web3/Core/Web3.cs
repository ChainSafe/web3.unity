#nullable enable
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Contracts;
using ChainSafe.Gaming.Evm.Contracts.BuiltIn;
using ChainSafe.Gaming.Evm.Providers;
using ChainSafe.Gaming.Evm.Signers;
using ChainSafe.Gaming.LocalStorage;
using ChainSafe.Gaming.RPC.Events;
using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Core;
using ChainSafe.Gaming.Web3.Core.Chains;
using ChainSafe.Gaming.Web3.Core.Evm;
using ChainSafe.Gaming.Web3.Core.Logout;
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
        private readonly IEventManager? events;
        private readonly ILogoutManager logoutManager;
        private readonly ILocalStorage localStorage;
        private readonly LifecycleManager lifecycleManager;

        private bool initialized;
        private bool terminated;

        internal Web3(ServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;

            rpcProvider = this.serviceProvider.GetService<IRpcProvider>();
            Chains = this.serviceProvider.GetRequiredService<IChainManager>();
            ProjectConfig = this.serviceProvider.GetRequiredService<IProjectConfig>();
            ChainConfig = this.serviceProvider.GetRequiredService<IChainConfig>();
            localStorage = this.serviceProvider.GetRequiredService<ILocalStorage>();
            lifecycleManager = this.serviceProvider.GetRequiredService<LifecycleManager>();
            ContractBuilder = this.serviceProvider.GetRequiredService<IContractBuilder>();
            Erc20 = this.serviceProvider.GetRequiredService<Erc20Service>();
            Erc721 = this.serviceProvider.GetRequiredService<Erc721Service>();
            Erc1155 = this.serviceProvider.GetRequiredService<Erc1155Service>();

            // These service are not readonly/lightweight (need a connected account).
            this.serviceProvider.TryGetService(out signer);
            this.serviceProvider.TryGetService(out transactionExecutor);
            this.serviceProvider.TryGetService(out logoutManager);
            this.serviceProvider.TryGetService(out events);
        }

        /// <summary>
        /// Access the project configuration object, providing access to project-specific settings.
        /// </summary>
        public IProjectConfig ProjectConfig { get; }

        /// <summary>
        /// Access the chain configuration object, providing access to blockchain-specific settings.
        /// </summary>
        public IChainConfig ChainConfig { get; }

        /// <summary>
        /// Access the <see cref="IRpcProvider"/> component, which provides RPC communication with the Ethereum network.
        /// </summary>
        public IRpcProvider RpcProvider => AssertComponentAccessible(rpcProvider);

        /// <summary>
        /// Access the <see cref="ISigner"/> component, responsible for signing transactions, messages, and providing the player's public address.
        /// </summary>
        public ISigner Signer => AssertComponentAccessible(signer);

        /// <summary>
        /// Access the <see cref="ITransactionExecutor"/> component, used for sending transactions to the blockchain.
        /// </summary>
        public ITransactionExecutor TransactionExecutor => AssertComponentAccessible(transactionExecutor);

        /// <summary>
        /// Access the Event Service of the Web3 instance, allowing you to subscribe to blockchain events.
        /// </summary>
        public IEventManager Events => AssertComponentAccessible(events);

        /// <summary>
        /// Access the Chain Manager of the Web3 instance to switch chains in runtime.
        /// </summary>
        public IChainManager Chains { get; }

        /// <summary>
        /// Access the factory for creating Ethereum smart contract wrappers.
        /// </summary>
        public IContractBuilder ContractBuilder { get; }

        /// <summary>
        /// Represents an ERC20 service that provides functionality to interact with ERC20 tokens.
        /// </summary>
        public Erc20Service Erc20 { get; }

        /// <summary>
        /// Represents an ERC721 service that provides functionality to interact with ERC721 tokens.
        /// </summary>
        public Erc721Service Erc721 { get; }

        /// <summary>
        /// Represents an ERC1155 service that provides functionality to interact with ERC1155 tokens.
        /// </summary>
        public Erc1155Service Erc1155 { get; }

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
            await localStorage.Initialize();

            await lifecycleManager.StartAsync();

            initialized = true;
        }

        /// <summary>
        /// Terminate this Web3 instance together with all of its components.
        /// </summary>
        /// <exception cref="Web3Exception">Web3 was already terminated.</exception>
        /// <returns>Task handle for the asynchronous process.</returns>
        public async ValueTask TerminateAsync(bool logout = false)
        {
            if (terminated)
            {
                throw new Web3Exception("Web3 was already terminated.");
            }

            if (logout)
            {
                await logoutManager.Logout();
            }

            if (initialized)
            {
                await lifecycleManager.StopAsync();
            }

            await serviceProvider.DisposeAsync();
            terminated = true;
        }

        public Task SwitchChain(string newChainId)
        {
            return Chains.SwitchChain(newChainId);
        }

        private T AssertComponentAccessible<T>(T? value)
            where T : notnull
        {
            string propertyName = typeof(T).Name;

            if (value == null)
            {
                throw new ServiceNotBoundWeb3Exception<T>(
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