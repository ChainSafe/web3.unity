using System.Collections.Generic;
using ChainSafe.Gaming.WalletConnect.Models;
using ChainSafe.Gaming.Web3.Environment;

namespace ChainSafe.Gaming.WalletConnect.Wallets
{
    public interface IWalletRegistry
    {
        IEnumerable<WalletModel> EnumerateSupportedWallets(Platform platform);

        WalletModel GetWallet(string name);
    }
}