using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Analytics;

namespace ChainSafe.Gaming.Tests.Core
{
    public class StubAnalyticsClient : IAnalyticsClient
    {
        public string AnalyticsVersion => "TESTS_STUB";

        public IChainConfig ChainConfig => null;

        public IProjectConfig ProjectConfig => null;

        public void CaptureEvent(AnalyticsEvent eventData)
        {
            // do nothing
        }
    }
}