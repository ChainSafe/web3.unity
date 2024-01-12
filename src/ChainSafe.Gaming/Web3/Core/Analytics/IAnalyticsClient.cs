using System.Threading.Tasks;

namespace ChainSafe.Gaming.Web3.Analytics
{
    /// <summary>
    /// Interface for the Analytics Client.
    /// </summary>
    public interface IAnalyticsClient
    {
        public string AnalyticsVersion { get; }

        IChainConfig ChainConfig { get; }

        IProjectConfig ProjectConfig { get; }

        /// <summary>
        /// Captures an analytics event.
        /// </summary>
        /// <param name="eventData">The analytics event data.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task CaptureEvent(AnalyticsEvent eventData);
    }
}