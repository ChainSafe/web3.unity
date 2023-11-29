using evm.net.Models;
using ImpromptuInterface;

namespace evm.net.Factory
{
    public class ImpromptuContractFactory : IContractFactory
    {
        public T BuildNewInstance<T>(IProvider provider, EvmAddress address) where T : class
        {
            return new Contract(provider, address, typeof(T)).ActLike<T>();
        }
    }
}