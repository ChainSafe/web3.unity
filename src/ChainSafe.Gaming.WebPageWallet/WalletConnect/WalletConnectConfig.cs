using System;
using System.Collections.Generic;
using WalletConnectSharp.Common.Logging;
using WalletConnectSharp.Core;

namespace ChainSafe.Gaming.Wallets.WalletConnect
{
    public struct WalletConnectConfig
    {
        public string ProjectId { get; set; }

        public string ProjectName { get; set; }

        public string BaseContext { get; set; }

        public Chain Chain { get; set; }

        public Metadata Metadata { get; set; }

        public ILogger Logger { get; set; }

        public bool IsMobilePlatform { get; set; }

        public Dictionary<string, Wallet> SupportedWallets { get; set; }
    }
}