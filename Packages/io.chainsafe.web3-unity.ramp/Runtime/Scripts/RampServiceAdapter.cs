using System.Collections;
using System.Collections.Generic;
using ChainSafe.Gaming.UnityPackage.Common;
using ChainSafe.Gaming.Web3.Build;
using UnityEngine;

namespace ChainSafe.Gaming.Exchangers.Ramp
{
    /// <summary>
    /// Add Ramp service when building a Web3 instance.
    /// </summary>
    public class RampServiceAdapter : MonoBehaviour, IWeb3BuilderServiceAdapter
    {
        [SerializeField] private RampExchangerConfigSO rampConfig;
        
        public Web3Builder ConfigureServices(Web3Builder web3Builder)
        {
            return web3Builder.Configure(services =>
            {
                services.UseRampExchanger(rampConfig);
            });
        }
    }
}
