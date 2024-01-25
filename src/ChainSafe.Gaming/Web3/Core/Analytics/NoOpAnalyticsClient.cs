using System.Threading.Tasks;

namespace ChainSafe.Gaming.Web3.Analytics
{
    public class NoOpAnalyticsClient : IAnalyticsClient
    {
        public NoOpAnalyticsClient(IChainConfig chainConfig, IProjectConfig projectConfig)
        {
            ChainConfig = chainConfig;
            ProjectConfig = projectConfig;
        }

        public string AnalyticsVersion => null;

        public IChainConfig ChainConfig { get; }

        public IProjectConfig ProjectConfig { get; }

        public void CaptureEvent(AnalyticsEvent eventData)
        {
        }
    }
}