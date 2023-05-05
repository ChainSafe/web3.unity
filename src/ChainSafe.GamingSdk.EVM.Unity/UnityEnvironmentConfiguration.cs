using System;
using ChainSafe.GamingSdk.Evm.Unity;

namespace ChainSafe.GamingWeb3.Unity
{
    [Serializable]
    public class UnityEnvironmentConfiguration
    {
        public DataDogAnalyticsConfiguration DataDog { get; set; }
    }
}