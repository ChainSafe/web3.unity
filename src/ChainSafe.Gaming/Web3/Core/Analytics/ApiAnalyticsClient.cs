using ChainSafe.Gaming.Web3.Environment;
using Newtonsoft.Json;

namespace ChainSafe.Gaming.Web3.Analytics
{
    internal class ApiAnalyticsClient : IAnalyticsClient
    {
        private const string LoggingUrl = "https://api.gaming.chainsafe.io/logging/logEvent";
        private const string AnalyticsVersion = "2.5";

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
            eventData.ChainId ??= chainConfig.ChainId;
            eventData.Network ??= chainConfig.Network;
            eventData.Version ??= AnalyticsVersion;

            await httpClient.PostRaw(LoggingUrl, JsonConvert.SerializeObject(eventData), "application/json");
        }
    }
}