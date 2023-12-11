using Nethereum.JsonRpc.Client;

namespace ChainSafe.Gaming.InProcessTransactionExecutor
{
    public interface IRpcClientWrapper
    {
        public IClient Client { get; }
    }
}