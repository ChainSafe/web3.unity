using ChainSafe.Gaming.Web3.Analytics;

namespace ChainSafe.Gaming.Tests.Core
{
    public class StubAnalyticsClient : IAnalyticsClient
    {
        public string AnalyticsVersion => "TESTS_STUB";

        public void CaptureEvent(AnalyticsEvent eventData)
        {
            // do nothing
        }
    }
}