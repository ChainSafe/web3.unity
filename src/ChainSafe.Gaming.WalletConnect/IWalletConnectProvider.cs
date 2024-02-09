using System.Threading.Tasks;

namespace ChainSafe.Gaming.WalletConnect
{
    /// <summary>
    /// Interface that represents the provider for the WalletConnect features.
    /// </summary>
    public interface IWalletConnectProvider
    {
        /// <summary>
        /// Connect WalletConnect session.
        /// </summary>
        /// <returns>Public address of the connected user.</returns>
        Task<string> Connect();

        /// <summary>
        /// Disconnect WalletConnect session.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task Disconnect();

        /// <summary>
        /// Make a request to the WalletConnect API.
        /// </summary>
        /// <param name="data">Request data.</param>
        /// <param name="expiry">Expiry in milliseconds.</param>
        /// <typeparam name="T">Type of the request.</typeparam>
        /// <returns>Response hash of the operation.</returns>
        Task<string> Request<T>(T data, long? expiry = null);
    }
}