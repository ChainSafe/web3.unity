using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Providers;

namespace ChainSafe.Gaming.Web3.Evm.Wallet
{
    /// <summary>
    /// Wallet Connection Provider.
    /// </summary>
    public interface IWalletProvider : IRpcProvider
    {
        /// <summary>
        /// Connect to a wallet.
        /// </summary>
        /// <returns>Connected address.</returns>
        Task<string> Connect();

        /// <summary>
        /// Disconnect from wallet.
        /// </summary>
        /// <returns>Awaitable disconnect Task.</returns>
        Task Disconnect();

        Task<T> Request<T>(string method, params object[] parameters);
    }
}