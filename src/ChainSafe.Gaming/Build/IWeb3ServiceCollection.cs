using Microsoft.Extensions.DependencyInjection;

namespace ChainSafe.Gaming.Build
{
    public interface IWeb3ServiceCollection : IServiceCollection
    {
    }

    public class Web3ServiceCollection : ServiceCollection, IWeb3ServiceCollection
    {
    }
}