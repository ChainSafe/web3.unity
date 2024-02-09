using ChainSafe.Gaming.Web3.Environment;

namespace ChainSafe.Gaming.NetCore
{
    /// <summary>
    /// Implementation of <see cref="IOperatingSystemMediator"/> for NetCore environment.
    /// </summary>
    public class NetCoreOperatingSystemMediator : IOperatingSystemMediator
    {
        public bool IsMobilePlatform => throw new System.NotImplementedException();

        public bool IsEditor => throw new System.NotImplementedException();

        public Platform Platform => throw new System.NotImplementedException();

        public string AppPersistentDataPath => throw new System.NotImplementedException();

        /// <summary>
        /// Open a Url using Http for .NetCore environment.
        /// </summary>
        /// <param name="url">Url to open.</param>
        /// <exception cref="System.NotImplementedException">Not Implemented.</exception>
        public void OpenUrl(string url) => throw new System.NotImplementedException();
    }
}