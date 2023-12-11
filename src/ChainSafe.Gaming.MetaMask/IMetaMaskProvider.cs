using System.Threading.Tasks;

namespace ChainSafe.Gaming.MetaMask
{
    /// <summary>
    /// Connect and disconnect to Metamask and make a Json RPC request.
    /// </summary>
    public interface IMetaMaskProvider
    {
        /// <summary>
        /// Connects to Metamask.
        /// </summary>
        /// <returns>Connected address.</returns>
        public Task<string> Connect();

        /// <summary>
        /// Make JsonRPC request using Metamask.
        /// </summary>
        /// <param name="method">JsonRPC method name.</param>
        /// <param name="parameters">JsonRPC request parameters.</param>
        /// <typeparam name="T">Type of response's result.</typeparam>
        /// <returns>Response's result.</returns>
        public Task<T> Request<T>(string method, params object[] parameters);
    }
}