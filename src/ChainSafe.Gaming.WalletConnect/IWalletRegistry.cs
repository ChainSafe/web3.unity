using System.Collections.Generic;
using ChainSafe.Gaming.WalletConnect.Models;
using ChainSafe.Gaming.Web3.Environment;

namespace ChainSafe.Gaming.WalletConnect
{
    public interface IWalletRegistry
    {
        IEnumerable<WalletConnectWalletModel> EnumerateSupportedWallets(Platform platform);

        WalletConnectWalletModel GetWallet(string name);
    }
}