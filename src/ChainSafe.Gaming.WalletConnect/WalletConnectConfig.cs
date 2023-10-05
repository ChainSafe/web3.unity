using System;
using System.Collections.Generic;
using WalletConnectSharp.Core;

namespace ChainSafe.Gaming.WalletConnect
{
    public class WalletConnectConfig
    {
        public string ProjectId { get; set; }

        public string ProjectName { get; set; }

        public string BaseContext { get; set; }

        public Chain Chain { get; set; }

        public Metadata Metadata { get; set; }

        public WalletConnectWalletModel DefaultWallet { get; set; }

        public Dictionary<string, WalletConnectWalletModel> SupportedWallets { get; set; }
    }
}