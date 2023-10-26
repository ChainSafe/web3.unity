using System.Threading.Tasks;

namespace ChainSafe.Gaming.WalletConnect
{
    /// <summary>
    /// Connect and disconnect to a wallet via Wallet Connect and make a Json RPC request.
    /// </summary>
    public interface IWalletConnectCustomProvider
    {
        /// <summary>
        /// Connects using Wallet Connect.
        /// </summary>
        /// <returns>Connected address.</returns>
        public Task<string> Connect();

        /// <summary>
        /// Make JsonRPC request using WalletConnect.
        /// </summary>
        /// <param name="data">Request data/body.</param>
        /// <param name="expiry">Expire time.</param>
        /// <typeparam name="T">Request DataType.</typeparam>
        /// <returns>Hash from the json rpc request.</returns>
        public Task<string> Request<T>(T data, long? expiry = null);

        /// <summary>
        /// Disconnect from a Wallet Connect Session.
        /// </summary>
        /// <returns>Disconnect async Task.</returns>
        public Task Disconnect();
    }
}