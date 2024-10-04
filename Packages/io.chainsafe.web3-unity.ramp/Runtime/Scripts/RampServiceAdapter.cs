using ChainSafe.Gaming.UnityPackage.Connection;
using ChainSafe.Gaming.Web3.Build;
using UnityEngine;

namespace ChainSafe.Gaming.Exchangers.Ramp
{
    /// <summary>
    ///     Add Ramp service when building a Web3 instance.
    /// </summary>
    public class RampServiceAdapter : MonoBehaviour, IServiceAdapter
    {
        [SerializeField] private RampExchangerConfigSO rampConfig;

        public Web3Builder ConfigureServices(Web3Builder web3Builder)
        {
#if !UNITY_EDITOR && (UNITY_WEBGL || UNITY_IOS)
            return web3Builder.Configure(services => { services.UseRampExchanger(rampConfig); });
#endif
            return web3Builder;
        }
    }
}