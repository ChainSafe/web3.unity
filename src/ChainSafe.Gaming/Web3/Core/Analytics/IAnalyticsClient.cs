namespace ChainSafe.Gaming.Web3.Analytics
{
    public interface IAnalyticsClient
    {
        void CaptureEvent(AnalyticsEvent eventData);
    }
}