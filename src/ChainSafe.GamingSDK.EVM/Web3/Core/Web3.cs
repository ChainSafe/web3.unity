#nullable enable
using System;
using System.Threading.Tasks;
using ChainSafe.GamingSDK.EVM.Web3.Core.Evm;
using Microsoft.Extensions.DependencyInjection;
using Web3Unity.Scripts.Library.Ethers.Providers;
using Web3Unity.Scripts.Library.Ethers.Signers;

namespace ChainSafe.GamingWeb3
{
    /// <summary>
    /// Facade for all Web3-related services.
    /// </summary>
    public class Web3 : IDisposable
    {
        private readonly ServiceProvider serviceProvider;
        private readonly IRpcProvider? provider;
        private readonly ISigner? signer;
        private readonly ITransactionExecutor? transactionExecutor;

        private bool initialized;
        private bool terminated;

        internal Web3(ServiceProvider serviceProvider, IRpcProvider? provider, ISigner? signer, ITransactionExecutor? transactionExecutor)
        {
            this.serviceProvider = serviceProvider;
            this.provider = provider;
            this.signer = signer;
            this.transactionExecutor = transactionExecutor;
        }

        public IRpcProvider Provider => AssertComponentAccessible(provider, nameof(Provider))!;

        public ISigner Signer => AssertComponentAccessible(signer, nameof(Signer))!;

        public ITransactionExecutor TransactionExecutor => AssertComponentAccessible(transactionExecutor, nameof(TransactionExecutor))!;

        private static T AssertComponentAccessible<T>(T value, string propertyName)
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

        void IDisposable.Dispose()
        {
            Terminate();
            GC.SuppressFinalize(this);
        }

        public async ValueTask Initialize()
        {
            if (initialized)
            {
                throw new Web3Exception("Web3 was already initialized.");
            }

            if (provider != null)
            {
                await provider.Initialize();
            }

            // todo initialize other components
            initialized = true;
        }

        public void Terminate()
        {
            if (terminated)
            {
                throw new Web3Exception("Web3 was already terminated.");
            }

            if (initialized)
            {
                // todo terminate other components
            }

            serviceProvider.Dispose();
            terminated = true;
        }
    }
}