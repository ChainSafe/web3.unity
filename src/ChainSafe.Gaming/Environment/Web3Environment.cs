using ChainSafe.Gaming.Analytics;

namespace ChainSafe.Gaming.Environment
{
    /// <summary>
    /// The environment Web3 is being executed in.
    /// </summary>
    public class Web3Environment
    {
        public Web3Environment(IHttpClient httpClient, ILogWriter logWriter, IAnalyticsClient analyticsClient, IOperatingSystemMediator operatingSystem)
        {
            OperatingSystem = operatingSystem;
            HttpClient = httpClient;
            LogWriter = logWriter;
            AnalyticsClient = analyticsClient;
        }

        public IHttpClient HttpClient { get; }

        public ILogWriter LogWriter { get; }

        public IAnalyticsClient AnalyticsClient { get; }

        public IOperatingSystemMediator OperatingSystem { get; }
    }
}