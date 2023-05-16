using System.Collections.Generic;
using ChainSafe.GamingWeb3.Environment;
using Web3Unity.Scripts.Library.Ethers.Unity;

namespace ChainSafe.GamingSdk.Evm.Unity
{
    public class DataDogAnalytics : IAnalyticsClient
    {
        private readonly DataDog dataDog;

        public DataDogAnalytics(DataDogAnalyticsConfiguration configuration)
        {
            dataDog = new DataDog(configuration.ApiKey, configuration.BaseUrl);
        }

        public void CaptureEvent(string eventName, Dictionary<string, object> properties)
        {
            dataDog.Capture(eventName, properties);
        }
    }
}