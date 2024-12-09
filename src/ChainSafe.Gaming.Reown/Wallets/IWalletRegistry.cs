using System.Collections.Generic;
using System.Collections.ObjectModel;
using ChainSafe.Gaming.Reown.Models;

namespace ChainSafe.Gaming.Reown.Wallets
{
    /// <summary>
    /// Registry of the wallets supported by Reown.
    /// </summary>
    public interface IWalletRegistry
    {
        /// <summary>
        /// Get all wallets supported on the current platform.
        /// </summary>
        /// <value>Sequence of <see cref="WalletModel"/> that the platform supports.</value>
        IEnumerable<WalletModel> SupportedWallets { get; }

        /// <summary>
        /// Return model for the wallet by it's ID.
        /// </summary>
        /// <param name="id">Wallet ID.</param>
        /// <returns>Wallet model.</returns>
        WalletModel GetWallet(string id);
    }
}