using ChainSafe.Gaming.Web3.Core.Chains;

namespace ChainSafe.Gaming.Web3.Analytics
{
    public class NoOpAnalyticsClient : IAnalyticsClient
    {
        private readonly IChainManager chainManager;

        public NoOpAnalyticsClient(IChainManager chainManager, IProjectConfig projectConfig)
        {
            this.chainManager = chainManager;
            ProjectConfig = projectConfig;
        }

        public string AnalyticsVersion => null;

        public IChainConfig ChainConfig => chainManager.Current;

        public IProjectConfig ProjectConfig { get; }

        public void CaptureEvent(AnalyticsEvent eventData)
        {
        }
    }
}