using ChainSafe.Gaming.EVM.Events;
using ChainSafe.Gaming.UnityPackage.Connection;
using ChainSafe.Gaming.Web3.Build;
using UnityEngine;


namespace ChainSafe.Gaming
{
    public class EventServiceAdapter : MonoBehaviour, IWeb3BuilderServiceAdapter
    {
        public Web3Builder ConfigureServices(Web3Builder web3Builder)
        {
            return web3Builder.Configure(services => { services.UseEvents(); });
        }
    }
}