using System;
using System.Collections.Generic;
using ChainSafe.Gaming.WalletConnect.Models;
using WalletConnectSharp.Core;

namespace ChainSafe.Gaming.WalletConnect
{
    [Serializable]
    public class WalletConnectConfig
    {
        public string SavedUserAddress { get; set; } = null;

        public string ProjectId { get; set; }

        public string ProjectName { get; set; }

        public string BaseContext { get; set; }

        public ChainModel Chain { get; set; }

        public Metadata Metadata { get; set; }

        public bool RedirectToWallet { get; set; }

        public WalletConnectWalletModel DefaultWallet { get; set; }

        public Dictionary<string, WalletConnectWalletModel> SupportedWallets { get; set; }
    }
}