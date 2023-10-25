namespace ChainSafe.Gaming.Web3.Analytics
{
    /// <summary>
    /// Interface for the Analytics Client.
    /// </summary>
    public interface IAnalyticsClient
    {
        /// <summary>
        /// Captures an analytics event.
        /// </summary>
        /// <param name="eventData">The analytics event data.</param>
        void CaptureEvent(AnalyticsEvent eventData);
    }
}