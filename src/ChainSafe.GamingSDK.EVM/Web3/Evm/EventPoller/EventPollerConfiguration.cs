using System;
using System.Collections.Generic;
using System.Text;

namespace ChainSafe.GamingSDK.EVM.Web3.Evm.EventPoller
{
    public class EventPollerConfiguration
    {
        public TimeSpan PollInterval { get; set; } = TimeSpan.FromSeconds(10);
    }
}
