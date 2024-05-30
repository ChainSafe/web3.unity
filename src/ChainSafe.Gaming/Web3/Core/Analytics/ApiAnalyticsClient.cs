using System.Threading.Tasks;
using ChainSafe.Gaming.Web3.Environment;
using Newtonsoft.Json;

namespace ChainSafe.Gaming.Web3.Analytics
{
    internal class ApiAnalyticsClient : IAnalyticsClient
    {
        private const string LoggingUrl = "https://api.gaming.chainsafe.io/logging/logEvent";

        private readonly IHttpClient httpClient;

        public ApiAnalyticsClient(IProjectConfig projectConfig, IChainConfig chainConfig, IHttpClient httpClient)
        {
            this.httpClient = httpClient;
            ChainConfig = chainConfig;
            ProjectConfig = projectConfig;
        }

        public IChainConfig ChainConfig { get; }

        public IProjectConfig ProjectConfig { get; }

        public string AnalyticsVersion => "2.5.5";

        public async void CaptureEvent(AnalyticsEvent eventData)
        {
            await httpClient.PostRaw(LoggingUrl, JsonConvert.SerializeObject(eventData), "application/json");
        }
    }
}