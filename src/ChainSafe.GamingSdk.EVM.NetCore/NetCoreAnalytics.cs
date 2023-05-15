using System.Collections.Generic;
using ChainSafe.GamingWeb3.Environment;

namespace Web3Unity.Scripts.Library.Ethers.NetCore
{
    public class NetCoreAnalytics : IAnalyticsClient
    {
        private readonly ILogWriter _logWriter;

        public NetCoreAnalytics(ILogWriter logWriter)
        {
            _logWriter = logWriter;
        }

        public void CaptureEvent(string eventName, Dictionary<string, object> properties)
        {
            var message = $"Tried capturing event {eventName}, but Analytics is not supported in Net.Core environment for now";
            _logWriter.Log(message);
        }
    }
}