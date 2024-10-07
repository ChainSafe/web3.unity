using System;
using System.Collections;
using System.Collections.Generic;
using ChainSafe.Gaming.EVM.Events;
using ChainSafe.Gaming.RPC.Events;
using ChainSafe.Gaming.Web3.Build;
using UnityEngine;
using UnityEngine.Serialization;

namespace ChainSafe.Gaming.UnityPackage.Connection
{
    public class EventsServiceAdapter : MonoBehaviour, ILightWeightServiceAdapter
    {
        [FormerlySerializedAs("pollingIntervalInSeconds")] [SerializeField, Tooltip("How often to poll/make requests to Rpc node in seconds in WebGL")]
        private float pollingInterval = 10f;
        
        public Web3Builder ConfigureServices(Web3Builder web3Builder)
        {
            return web3Builder.Configure(services =>
            {
                services.UseEvents(new PollingEventManagerConfig { PollInterval = TimeSpan.FromSeconds(pollingInterval) });
            });
        }
    }
}
