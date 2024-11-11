using System;

namespace ChainSafe.Gaming.RPC.Events
{
    public class PollingEventManagerConfig
    {
        public TimeSpan PollInterval { get; set; } = TimeSpan.FromSeconds(10);

        public bool ForceEventPolling { get; set; } = false;
    }
}
