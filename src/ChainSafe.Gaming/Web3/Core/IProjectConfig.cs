namespace ChainSafe.Gaming.Web3
{
    /// <summary>
    /// Configuration object containing project settings.
    /// </summary>
    public interface IProjectConfig
    {
        /// <summary>
        /// The project id issued by ChainSafe. Follow https://docs.gaming.chainsafe.io to learn more.
        /// </summary>
        public string ProjectId { get; }

        public bool EnableAnalytics => true;
    }
}