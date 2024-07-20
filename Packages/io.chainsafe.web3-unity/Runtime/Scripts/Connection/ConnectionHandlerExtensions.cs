using ChainSafe.Gaming.UnityPackage.Connection;
using ChainSafe.Gaming.Web3.Build;

namespace ChainSafe.Gaming.Connection
{
    public static class ConnectionHandlerExtensions
    {
        public static Web3Builder ConfigureServices(this Web3Builder web3Builder, IWeb3BuilderServiceAdapter adapter)
        {
            return adapter.ConfigureServices(web3Builder);
        }
        
        public static Web3Builder ConfigureServices(this Web3Builder web3Builder, IWeb3BuilderServiceAdapter[] adapters)
        {
            foreach (var adapter in adapters)
            {
                web3Builder = adapter.ConfigureServices(web3Builder);
            }

            return web3Builder;
        }
    }
}