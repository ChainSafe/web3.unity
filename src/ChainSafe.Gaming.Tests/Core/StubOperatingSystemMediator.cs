using ChainSafe.Gaming.Web3.Environment;

namespace ChainSafe.Gaming.Tests.Core
{
    public class StubOperatingSystemMediator : IOperatingSystemMediator
    {
        public StubOperatingSystemMediator()
        {
            Platform = Platform.Desktop;
            AppPersistentDataPath = "data/";
        }

        public StubOperatingSystemMediator(Platform platform, string applicationDataPath)
        {
            Platform = platform;
            AppPersistentDataPath = applicationDataPath;
        }

        public bool IsMobilePlatform => Platform is Platform.Android or Platform.IOS;

        public bool IsEditor => false;

        public Platform Platform { get; set; }

        public string AppPersistentDataPath { get; set; }

        public void OpenUrl(string url)
        {
            throw new System.NotImplementedException();
        }
    }
}