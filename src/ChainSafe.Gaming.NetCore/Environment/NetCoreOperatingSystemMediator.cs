using ChainSafe.Gaming.Environment;

namespace ChainSafe.Gaming.NetCore.Environment
{
    public class NetCoreOperatingSystemMediator : IOperatingSystemMediator
    {
        public string ClipboardContent
        {
            get => throw new System.NotImplementedException();
            set => throw new System.NotImplementedException();
        }

        public void OpenUrl(string url) => throw new System.NotImplementedException();
    }
}