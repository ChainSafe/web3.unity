using ChainSafe.Gaming.Web3.Environment;

namespace ChainSafe.Gaming.Tests.Core
{
    public class StubOperatingSystemMediator : IOperatingSystemMediator
    {
        public StubOperatingSystemMediator()
        {
            Platform = Platform.Desktop;
            ApplicationDataPath = "data/";
        }

        public StubOperatingSystemMediator(Platform platform, string applicationDataPath)
        {
            Platform = platform;
            ApplicationDataPath = applicationDataPath;
        }

        public bool IsMobilePlatform => Platform is Platform.Android or Platform.IOS;

        public Platform Platform { get; set; }

        public string ApplicationDataPath { get; set; }

        public void OpenUrl(string url)
        {
            throw new System.NotImplementedException();
        }
    }
}