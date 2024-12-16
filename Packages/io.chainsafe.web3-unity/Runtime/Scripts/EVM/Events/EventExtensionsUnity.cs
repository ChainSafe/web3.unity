using ChainSafe.Gaming.RPC.Events;
using ChainSafe.Gaming.Web3.Build;

namespace ChainSafe.Gaming.EVM.Events
{
    public static class EventExtensionsUnity
    {
        /// <summary>
        /// Enable EVM Event Manager for the Web3 client.
        /// </summary>
        /// <param name="services">The Web3 services collection.</param>
        /// <param name="eventPollerConfig">(Optional) Event Poller configuration. This will only be used for the WebGL platform.</param>
        /// <returns></returns>
        /// <remarks>
        /// For all the platforms that support multithreading the WebSocket strategy will be used.
        /// In WebGL this will bind the Polling strategy which will fetch data periodically.
        /// </remarks>
        public static IWeb3ServiceCollection UseEvents(this IWeb3ServiceCollection services, PollingEventManagerConfig eventPollerConfig = null)
        {
#if !UNITY_WEBGL || UNITY_EDITOR

            if (eventPollerConfig is { ForceEventPolling: true })
                services.UseEventsWithPolling();
            else
                services.UseEventsWithWebSocket();
#else
            if (eventPollerConfig == null)
            {
                services.UseEventsWithPolling();
            }
            else
            {
                services.UseEventsWithPolling(eventPollerConfig);
            }
#endif
            return services;
        }
    }
}