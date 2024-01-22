#nullable enable
using System.Collections.Generic;
using ChainSafe.Gaming.WalletConnect.Connection;
using WalletConnectSharp.Core;
using WalletConnectSharp.Network.Interfaces;

namespace ChainSafe.Gaming.WalletConnect
{
    public interface IWalletConnectConfig
    {
        public bool RememberSession { get; }

        public bool ForceNewSession { get; }

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