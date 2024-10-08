using System.Collections.Generic;
using ChainSafe.Gaming.UnityPackage.Connection;
using ChainSafe.Gaming.Web3.Build;

namespace ChainSafe.Gaming.Connection
{
    /// <summary>
    /// Add extension methods that are used to configure services for <see cref="Web3"/>.
    /// </summary>
    public static class ConnectionHandlerExtensions
    {
        /// <summary>
        /// Add the given adapter to the <see cref="Web3Builder"/>.
        /// </summary>
        /// <param name="web3Builder">Web3Builder instance to be configured.</param>
        /// <param name="adapter"><see cref="IServiceAdapter"/> to be added to <see cref="Web3Builder"/>.</param>
        /// <returns>Configured <see cref="Web3Builder"/>.</returns>
        public static Web3Builder ConfigureServices(this Web3Builder web3Builder, IServiceAdapter adapter)
        {
            return adapter.ConfigureServices(web3Builder);
        }

        /// <summary>
        /// Add the given adapters to the <see cref="Web3Builder"/>.
        /// </summary>
        /// <param name="web3Builder">Web3Builder instance to be configured.</param>
        /// <param name="adapters">Multiple <see cref="IServiceAdapter"/> to be added to <see cref="Web3Builder"/>.</param>
        /// <returns>Configured <see cref="Web3Builder"/>.</returns>
        public static Web3Builder ConfigureServices(this Web3Builder web3Builder, IEnumerable<IServiceAdapter> adapters)
        {
            foreach (var adapter in adapters)
            {
                web3Builder = adapter.ConfigureServices(web3Builder);
            }

            return web3Builder;
        }
    }
}