namespace ChainSafe.Gaming.Web3.Analytics
{
    internal class NoOpAnalyticsClient : IAnalyticsClient
    {
        public void CaptureEvent(AnalyticsEvent eventData)
        {
            // ignore everything. analytics is disabled
        }
    }
}