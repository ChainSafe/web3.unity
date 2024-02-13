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
        /// <summary>
        /// Is platform on Mobile or not.
        /// </summary>
        public bool IsMobilePlatform { get; }

        /// <summary>
        /// Is running in Editor or not.
        /// </summary>
        public bool IsEditor { get; }

        /// <summary>
        /// Current Platform enum.
        /// </summary>
        public Platform Platform { get; }

        /// <summary>
        /// The path to the folder where data can be stored persistently.
        /// </summary>
        public string AppPersistentDataPath { get; }

        /// <summary>
        /// Opens URL.
        /// </summary>
        public void OpenUrl(string url);
    }
}