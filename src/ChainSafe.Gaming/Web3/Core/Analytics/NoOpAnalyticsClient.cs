using System.Threading.Tasks;

namespace ChainSafe.Gaming.Web3.Analytics
{
    internal class NoOpAnalyticsClient : IAnalyticsClient
    {
        public string AnalyticsVersion => null;

        public Task CaptureEvent(AnalyticsEvent eventData)
        {
            return Task.CompletedTask;
        }
    }
}