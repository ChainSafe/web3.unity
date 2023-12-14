using System;

namespace Plugins.CountlySDK.Models
{
    [Serializable]
    public class CountlyAuthModel
    {
        public string ServerUrl;
        public string AppKey;
        public string DeviceId;
    }
}