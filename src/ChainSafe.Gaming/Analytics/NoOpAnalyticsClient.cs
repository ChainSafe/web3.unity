namespace ChainSafe.Gaming.Analytics
{
    public class NoOpAnalyticsClient : IAnalyticsClient
    {
        public void CaptureEvent(AnalyticsEvent eventData)
        {
            // ignore
        }
    }
}