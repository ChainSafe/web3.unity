#nullable enable
using System.Collections.Generic;
using WalletConnectSharp.Core;
using WalletConnectSharp.Network.Interfaces;

namespace ChainSafe.Gaming.WalletConnect
{
    public interface IWalletConnectConfigNew
    {
        public bool RememberSession { get; }

        public bool ForceNewSession { get; } // todo remove?

        bool AutoRenewSession { get; }

        string ProjectName { get; }

        string ProjectId { get; }

        string BaseContext { get; }

        IConnectionBuilder ConnectionBuilder { get; }

        Metadata Metadata { get; }

        string StoragePath { get; }

        IList<string>? EnabledWallets { get; }

        IList<string>? DisabledWallets { get; }

        WalletLocationOptions WalletLocationOptions { get; }

        IConnectionHandlerProvider ConnectionHandlerProvider { get; }

        string? OverrideRegistryUri { get; }
    }
}