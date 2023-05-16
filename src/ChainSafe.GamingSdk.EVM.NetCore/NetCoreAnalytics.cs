using System.Collections.Generic;
using ChainSafe.GamingWeb3.Environment;

namespace Web3Unity.Scripts.Library.Ethers.NetCore
{
    public class NetCoreAnalytics : IAnalyticsClient
    {
        public NetCoreAnalytics()
        {
        }

        public void CaptureEvent(string eventName, Dictionary<string, object> properties)
        {
            // TODO: add an analytics solution for netcore
        }
    }
}