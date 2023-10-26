using Microsoft.Extensions.DependencyInjection;

namespace ChainSafe.Gaming.Web3.Build
{
    /// <summary>
    /// Collection of services to register in the Web3 dependency injection system.
    /// </summary>
    public interface IWeb3ServiceCollection : IServiceCollection
    {
    }

    /// <summary>
    /// Collection of services to register in the Web3 dependency injection system.
    /// </summary>
    public class Web3ServiceCollection : ServiceCollection, IWeb3ServiceCollection
    {
    }
}