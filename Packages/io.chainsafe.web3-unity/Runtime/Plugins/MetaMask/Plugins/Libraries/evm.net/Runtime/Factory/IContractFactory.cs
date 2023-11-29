using evm.net.Models;

namespace evm.net.Factory
{
    public interface IContractFactory
    {
        T BuildNewInstance<T>(IProvider provider, EvmAddress address = null) where T : class;
    }
}