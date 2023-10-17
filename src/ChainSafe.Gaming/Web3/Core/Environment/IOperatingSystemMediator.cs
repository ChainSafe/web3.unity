namespace ChainSafe.Gaming.Web3.Environment
{
    public enum Platform
    {
        Editor,
        Android,
        IOS,
        WebGL,
        Desktop,
    }

    public interface IOperatingSystemMediator
    {
        public bool IsMobilePlatform { get; }

        public Platform Platform { get; }

        public void OpenUrl(string url);
    }
}