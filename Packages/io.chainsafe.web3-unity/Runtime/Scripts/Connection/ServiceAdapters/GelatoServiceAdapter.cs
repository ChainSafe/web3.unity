using ChainSafe.Gaming.Web3.Build;
using ChainSafe.GamingSdk.Gelato;
using UnityEngine;

namespace ChainSafe.Gaming.UnityPackage.Connection
{
    /// <summary>
    /// Enables usage of Gelato when attached.
    /// </summary>
    public class GelatoServiceAdapter : MonoBehaviour, IServiceAdapter
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
