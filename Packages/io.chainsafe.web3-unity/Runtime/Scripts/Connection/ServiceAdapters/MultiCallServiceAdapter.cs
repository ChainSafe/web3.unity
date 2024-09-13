using ChainSafe.Gaming.MultiCall;
using ChainSafe.Gaming.Web3.Build;
using UnityEngine;

namespace ChainSafe.Gaming.UnityPackage.Connection
{
    public class MultiCallServiceAdapter : MonoBehaviour, IWeb3BuilderServiceAdapter
    {
        public Web3Builder ConfigureServices(Web3Builder web3Builder)
        {
            return web3Builder.Configure(services =>
            {
                services.UseMultiCall();
            });
        }
    }
}
