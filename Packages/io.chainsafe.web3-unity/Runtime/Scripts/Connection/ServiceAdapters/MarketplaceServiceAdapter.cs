using ChainSafe.Gaming.Marketplace.Extensions;
using ChainSafe.Gaming.Web3.Build;
using UnityEngine;

namespace ChainSafe.Gaming.UnityPackage.Connection
{
    /// <summary>
    /// Enables usage of Marketplace when attached.
    /// </summary>
    public class MarketplaceServiceAdapter : MonoBehaviour, IWeb3BuilderServiceAdapter
    {
        public Web3Builder ConfigureServices(Web3Builder web3Builder)
        {
            return web3Builder.Configure(services =>
            {
                services.UseMarketplace();
            });
        }
    }
}
