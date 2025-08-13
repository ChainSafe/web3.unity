using ChainSafe.Gaming.Web3.Core.Chains;

namespace ChainSafe.Gaming.Web3.Analytics
{
    public class NoOpAnalyticsClient : IAnalyticsClient
    {
        private readonly IChainManager chainManager;

        public NoOpAnalyticsClient(IChainManager chainManager)
        {
            this.chainManager = chainManager;
        }

        public string AnalyticsVersion => null;

        public IChainConfig ChainConfig => chainManager.Current;

        public void CaptureEvent(AnalyticsEvent eventData)
        {
        }
    }
}
