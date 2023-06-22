namespace ChainSafe.Gaming.Analytics
{
    public interface IAnalyticsClient
    {
        void CaptureEvent(AnalyticsEvent eventData);
    }
}