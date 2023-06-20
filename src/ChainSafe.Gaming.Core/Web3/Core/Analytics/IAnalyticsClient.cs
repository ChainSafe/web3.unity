namespace ChainSafe.GamingWeb3.Analytics
{
    public interface IAnalyticsClient
    {
        void CaptureEvent(AnalyticsEvent eventData);
    }
}