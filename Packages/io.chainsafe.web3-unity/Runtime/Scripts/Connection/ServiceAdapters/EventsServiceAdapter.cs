using System;
using ChainSafe.Gaming.EVM.Events;
using ChainSafe.Gaming.RPC.Events;
using ChainSafe.Gaming.Web3.Build;
using UnityEngine;

namespace ChainSafe.Gaming.UnityPackage.Connection
{
    public class EventsServiceAdapter : MonoBehaviour, ILightWeightServiceAdapter
    {
        [SerializeField, Tooltip("Enable this if a WebSocket connection is unavailable or if the blockchain does not provide a WebSocket URL")]
        private bool forceEventPolling;

        [SerializeField, Tooltip("How often to poll/make requests to Rpc node in seconds in WebGL")]
        private float pollingInterval = 10f;


        public Web3Builder ConfigureServices(Web3Builder web3Builder)
        {
            return web3Builder.Configure(services =>
            {
                services.UseEvents(new PollingEventManagerConfig
                {
                    PollInterval = TimeSpan.FromSeconds(pollingInterval),
                    ForceEventPolling = forceEventPolling
                });
            });
        }
    }
}
