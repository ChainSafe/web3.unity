#nullable enable
using System.Collections.Generic;
using WalletConnectSharp.Core;
using WalletConnectSharp.Network.Interfaces;

namespace ChainSafe.Gaming.WalletConnect
{
    public class WalletConnectConfigNew : IWalletConnectConfigNew
    {
        public bool RememberSession { get; set; }

        public bool AutoRenewSession { get; set; } = true;

        public string ProjectName { get; set; }

        public string ProjectId { get; set; }

        public string BaseContext { get; set; }

        public IConnectionBuilder ConnectionBuilder { get; set; }

        public Metadata Metadata { get; set; }

        public string StoragePath { get; set; }

        public IList<string>? EnabledWallets { get; set; } // todo note in comments that some supported wallets are wierd and it's better to decide on which wallets you want to use or don't want to use 

        public IList<string>? DisabledWallets { get; set; }

        public WalletLocationOptions WalletLocationOptions { get; set; }

        public IConnectionDialogProvider ConnectionDialogProvider { get; set; }

        public string? OverrideRegistryUri { get; set; }
    }
}