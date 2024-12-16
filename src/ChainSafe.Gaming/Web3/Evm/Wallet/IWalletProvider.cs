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

        /// <summary>
        /// Method that automatically adds a new network to the wallet if it's not present.
        /// If the network is present, it prompts to switch to that network.
        /// </summary>
        /// <exception cref="InvalidOperationException">Throwing exception if we fail to switch to the network.</exception>
        /// <returns>Nothing.</returns>
        Task SwitchChain(IChainConfig chainConfig = null);

        /// <summary>
        /// Fetches the current Chain ID of the connected wallet.
        /// </summary>
        /// <returns>The current Chain ID of the connected wallet.</returns>
        Task<string> GetWalletChainId();
    }
}