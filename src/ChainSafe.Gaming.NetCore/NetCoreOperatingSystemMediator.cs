using ChainSafe.Gaming.Web3.Environment;

namespace ChainSafe.Gaming.NetCore
{
    public class NetCoreOperatingSystemMediator : IOperatingSystemMediator
    {
        public bool IsMobilePlatform => throw new System.NotImplementedException();

        public Platform Platform => throw new System.NotImplementedException();

        public void OpenUrl(string url) => throw new System.NotImplementedException();
    }
}