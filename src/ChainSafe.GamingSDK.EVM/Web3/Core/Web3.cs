#nullable enable
using System;
using System.Threading.Tasks;
using ChainSafe.GamingWeb3.Environment;
using ChainSafe.GamingWeb3.Evm;
using ChainSafe.GamingWeb3.Evm.Signers;
using Microsoft.Extensions.DependencyInjection;

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
    private readonly IEvmWallet? _wallet;
    
    private bool _initialized;
    private bool _terminated;

    public IWeb3Environment Environment { get; }
    public IEvmProvider Provider => AssertComponentAccessible(_provider, nameof(Provider))!;
    public IEvmSigner Signer => AssertComponentAccessible(_signer, nameof(Signer))!;
    public IEvmWallet Wallet => AssertComponentAccessible(_wallet, nameof(Wallet))!;

    internal Web3(ServiceProvider serviceProvider, IWeb3Environment environment, IEvmProvider? provider = null, IEvmSigner? signer = null, IEvmWallet? wallet = null)
    {
      Environment = environment;
      _serviceProvider = serviceProvider;
      _provider = provider;
      _signer = signer;
      _wallet = wallet;
    }

    void IDisposable.Dispose() => Terminate();

    public async ValueTask Initialize()
    {
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
      
      if (!_initialized)
      {
        throw new Web3Exception($"Can't access {propertyName}. Initialize Web3 first.");
      }

      return value;
    }
  }
}