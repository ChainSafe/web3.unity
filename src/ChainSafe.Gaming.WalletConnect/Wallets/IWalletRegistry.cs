using System.Collections.Generic;
using ChainSafe.Gaming.WalletConnect.Models;
using ChainSafe.Gaming.Web3.Environment;

namespace ChainSafe.Gaming.WalletConnect.Wallets
{
    /// <summary>
    /// Registry of the wallets supported by WalletConnect.
    /// </summary>
    public interface IWalletRegistry
    {
        /// <summary>
        /// Return model for the wallet by it's name.
        /// </summary>
        /// <param name="name">The name of the wallet.</param>
        /// <returns>Wallet model.</returns>
        WalletModel GetWallet(string name);

        /// <summary>
        /// Enumerates through all wallets supported on the specified platform.
        /// </summary>
        /// <param name="platform">The platform.</param>
        /// <returns>Sequence of <see cref="WalletModel"/> that the platform supports.</returns>
        IEnumerable<WalletModel> EnumerateSupportedWallets(Platform platform);
    }
}