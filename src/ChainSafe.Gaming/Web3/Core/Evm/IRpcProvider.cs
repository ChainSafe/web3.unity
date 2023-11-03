using System.Threading.Tasks;

namespace ChainSafe.Gaming.Evm.Providers
{
    /// <summary>
    /// Represents an interface for an RPC (Remote Procedure Call) provider in a blockchain environment.
    /// </summary>
    /// <remarks>
    /// RPC provider is a core component that allows to communicate with
    /// a blockchain network using remote procedure calls,
    /// enabling actions such as querying blockchain data.
    /// </remarks>
    public interface IRpcProvider
    {
        /// <summary>
        /// Gets the last known blockchain network information.
        /// </summary>
        Network.Network LastKnownNetwork { get; }

        /// <summary>
        /// Asynchronously refreshes and retrieves the current blockchain network information.
        /// </summary>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous operation. The task result contains
        /// the updated <see cref="Network.Network"/> object representing the current network.
        /// </returns>
        Task<Network.Network> RefreshNetwork();

        /// <summary>
        /// Asynchronously performs a specific RPC method on the blockchain.
        /// </summary>
        /// <typeparam name="T">The expected return type of the RPC method.</typeparam>
        /// <param name="method">The name of the RPC method to execute.</param>
        /// <param name="parameters">An array of parameters to pass to the RPC method.</param>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous operation. The task result contains
        /// the result of the RPC method, deserialized to the specified return type <typeparamref name="T"/>.
        /// </returns>
        Task<T> Perform<T>(string method, params object[] parameters);
    }
}