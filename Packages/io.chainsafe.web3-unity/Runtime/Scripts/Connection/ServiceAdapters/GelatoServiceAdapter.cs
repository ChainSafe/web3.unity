using ChainSafe.Gaming.Web3.Build;
using ChainSafe.GamingSdk.Gelato;
using UnityEngine;

namespace ChainSafe.Gaming.UnityPackage.Connection
{
    public class GelatoServiceAdapter : MonoBehaviour, IWeb3BuilderServiceAdapter
    {
        [SerializeField] private string gelatoApiKey;
        
        public Web3Builder ConfigureServices(Web3Builder web3Builder)
        {
            return web3Builder.Configure(services =>
            {
                services.UseGelato(gelatoApiKey);
            });
        }
    }
}
