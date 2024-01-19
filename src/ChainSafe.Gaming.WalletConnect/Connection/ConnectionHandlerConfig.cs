using System;
using System.Collections.Generic;
using ChainSafe.Gaming.WalletConnect.Models;

namespace ChainSafe.Gaming.WalletConnect.Connection
{
    public class ConnectionHandlerConfig
    {
        public WalletLocationOptions WalletLocationOptions { get; set; }

        public List<WalletModel> LocalWalletOptions { get; set; }

        public OpenLocalWalletDelegate RedirectToWallet { get; set; }

        public bool DelegateLocalWalletSelectionToOs { get; set; }

        public string ConnectRemoteWalletUri { get; set; }

        public Action RedirectOsManaged { get; set; }
    }
}