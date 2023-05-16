using System.Collections.Generic;

namespace ChainSafe.GamingWeb3.Environment
{
    public interface IAnalyticsClient
    {
        void CaptureEvent(string eventName, Dictionary<string, object> properties);
    }
}