namespace ChainSafe.Gaming.Web3.Analytics
{
    public class NoOpAnalyticsClient : IAnalyticsClient
    {
        public void CaptureEvent(AnalyticsEvent eventData)
        {
            // ignore everything. analytics is disabled
        }
    }
}