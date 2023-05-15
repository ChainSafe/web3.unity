#nullable enable
using System;
using System.Threading.Tasks;
using Web3Unity.Scripts.Library.Ethers;
using Web3Unity.Scripts.Library.Ethers.Signers;
using Microsoft.Extensions.DependencyInjection;
using Web3Unity.Scripts.Library.Ethers.Providers;

namespace ChainSafe.GamingWeb3
{
    /// <summary>
    /// Facade for all Web3-related services
    /// </summary>
    public class Web3 : IDisposable
    {
        private readonly ServiceProvider _serviceProvider;
        private readonly IEvmProvider? _provider;
        private readonly IEvmSigner? _signer;

        private bool _initialized;
        private bool _terminated;

        public IEvmProvider Provider => AssertComponentAccessible(_provider, nameof(Provider))!;
        public IEvmSigner Signer => AssertComponentAccessible(_signer, nameof(Signer))!;

        internal Web3(ServiceProvider serviceProvider, IEvmProvider? provider = null, IEvmSigner? signer = null)
        {
            _serviceProvider = serviceProvider;
            _provider = provider;
            _signer = signer;
        }

        void IDisposable.Dispose() => Terminate();

        public async ValueTask Initialize()
        {
            if (_initialized) throw new Web3Exception("Web3 was already initialized.");
            if (_provider != null) await _provider.Initialize();

            // todo initialize other components

            _initialized = true;
        }

        public void Terminate()
        {
            if (_terminated) throw new Web3Exception("Web3 was already terminated.");

            if (_initialized)
            {
                // todo terminate other components
            }

            _serviceProvider.Dispose();
            _terminated = true;
        }

        private T AssertComponentAccessible<T>(T value, string propertyName)
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
    }
}