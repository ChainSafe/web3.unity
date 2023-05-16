namespace ChainSafe.GamingWeb3.Environment
{
    /// <summary>
    /// The environment Web3 is being executed in.
    /// </summary>
    public class Web3Environment
    {
        public Web3Environment(ISettingsProvider settingsProvider, IHttpClient httpClient, ILogWriter logWriter, IAnalyticsClient analyticsClient)
        {
            SettingsProvider = settingsProvider;
            HttpClient = httpClient;
            LogWriter = logWriter;
            AnalyticsClient = analyticsClient;
        }

        public ISettingsProvider SettingsProvider { get; }

        public IHttpClient HttpClient { get; }

        public ILogWriter LogWriter { get; }

        public IAnalyticsClient AnalyticsClient { get; }
    }
}