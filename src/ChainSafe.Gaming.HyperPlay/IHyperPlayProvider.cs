using System.Threading.Tasks;

namespace ChainSafe.Gaming.HyperPlay
{
    /// <summary>
    /// HyperPlay provider for connecting wallet and making RPC requests via HyperPlay desktop client and.
    /// </summary>
    public interface IHyperPlayProvider
    {
        /// <summary>
        /// Connect to a wallet via HyperPlay desktop client.
        /// </summary>
        /// <returns>Connected public address.</returns>
        public Task<string> Connect();

        /// <summary>
        /// Make RPC request to HyperPlay desktop client.
        /// This prompts user to approve a request on HyperPlay desktop client.
        /// </summary>
        /// <param name="method">RPC method name.</param>
        /// <param name="parameters">RPC request parameters.</param>
        /// <typeparam name="T">Type of request's response.</typeparam>
        /// <returns>RPC request's Response.</returns>
        public Task<T> Request<T>(string method, params object[] parameters);
    }
}