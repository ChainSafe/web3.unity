namespace ChainSafe.GamingWeb3.Analytics
{
    public class NoOpAnalyticsClient : IAnalyticsClient
    {
        public void CaptureEvent(AnalyticsEvent eventData)
        {
            // ignore
        }
    }
}