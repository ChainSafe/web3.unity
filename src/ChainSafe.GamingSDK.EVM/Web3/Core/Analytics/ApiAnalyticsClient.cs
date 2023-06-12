using ChainSafe.GamingWeb3.Environment;

namespace ChainSafe.GamingWeb3.Analytics
{
    public class ApiAnalyticsClient : IAnalyticsClient
    {
        private const string LoggingUrl = "https://api.gaming.chainsafe.io/logging/logEvent";

        private readonly IProjectConfig projectConfig;
        private readonly IChainConfig chainConfig;
        private readonly IHttpClient httpClient;

        public ApiAnalyticsClient(IProjectConfig projectConfig, IChainConfig chainConfig, IHttpClient httpClient)
        {
            this.httpClient = httpClient;
            this.chainConfig = chainConfig;
            this.projectConfig = projectConfig;
        }

        public async void CaptureEvent(AnalyticsEvent eventData)
        {
            eventData.ProjectId = projectConfig.ProjectId;
            eventData.ChainId ??= chainConfig.Chain;
            eventData.Rpc ??= chainConfig.Rpc;

            var data = $"chain={eventData.ChainId}&network={eventData.Rpc}&gameData={eventData}";

            await httpClient.PostRaw(LoggingUrl, data, "application/x-www-form-urlencoded");
        }
    }
}