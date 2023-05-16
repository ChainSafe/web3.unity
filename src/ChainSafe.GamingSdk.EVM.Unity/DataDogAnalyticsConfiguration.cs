using System;

namespace ChainSafe.GamingSdk.Evm.Unity
{
    [Serializable]
    public class DataDogAnalyticsConfiguration
    {
        /// <summary>
        /// Your unique api key for DataDog.
        /// </summary>
        public string ApiKey { get; set; }

        /// <summary>
        /// (Optional) TODO.
        /// </summary>
        public string BaseUrl { get; set; }
    }
}