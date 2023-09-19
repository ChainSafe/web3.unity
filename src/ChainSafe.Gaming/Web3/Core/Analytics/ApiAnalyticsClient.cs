using ChainSafe.Gaming.Web3.Environment;
using Newtonsoft.Json;

namespace ChainSafe.Gaming.Web3.Analytics
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
            eventData.ProjectId ??= projectConfig.ProjectId;
            eventData.ChainId ??= chainConfig.Chain;
            eventData.Network ??= chainConfig.Network;

            await httpClient.PostRaw(LoggingUrl, JsonConvert.SerializeObject(eventData), "application/json");
        }
    }
}