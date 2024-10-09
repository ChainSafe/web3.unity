#nullable enable
using System.Collections.Generic;
using ChainSafe.Gaming.WalletConnect.Connection;
using ChainSafe.Gaming.Web3.Evm.Wallet;
using WalletConnectSharp.Core;
using WalletConnectSharp.Network.Interfaces;

namespace ChainSafe.Gaming.WalletConnect
{
    /// <summary>
    /// Interface for WalletConnect configuration.
    /// </summary>
    public interface IWalletConnectConfig : IWalletProviderConfig
    {
        /// <summary>
        /// Set to true if you want to store this session on a disk for the next time.
        /// </summary>
        public bool RememberSession { get; }

        /// <summary>
        /// Enforce new session even if the stored one is available.
        /// </summary>
        public bool ForceNewSession { get; }

        /// <summary>
        /// Automatically renew session when expired.
        /// </summary>
        bool AutoRenewSession { get; }

        /// <summary>
        /// Name of your project.
        /// </summary>
        string ProjectName { get; }

        /// <summary>
        /// Project ID provided to you by WalletConnect.
        /// </summary>
        string ProjectId { get; }

        /// <summary>
        /// Name of the context to be used by WalletConnect.
        /// </summary>
        string BaseContext { get; }

        /// <summary>
        /// Used to set custom <see cref="IConnectionBuilder"/> to support Unity versions before 2022.1.
        /// </summary>
        IConnectionBuilder ConnectionBuilder { get; }

        /// <summary>
        /// WalletConnect metadata.
        /// </summary>
        Metadata Metadata { get; }

        /// <summary>
        /// Relative path to folder where storage files should be place.
        /// </summary>
        /// <remarks>The path is relative to the app's data folder.</remarks>
        string StoragePath { get; }

        /// <summary>
        /// List of the wallets that you want to show to your user as local wallet connect options.
        /// Keep this empty to enable all of the wallets supported for the platform.
        /// </summary>
        /// <remarks>
        /// Use either <see cref="EnabledWallets"/> or <see cref="DisabledWallets"/>.
        /// If both are provided <see cref="DisabledWallets"/> will be ignored.
        /// </remarks>
        IList<string>? EnabledWallets { get; }

        /// <summary>
        /// List of the wallets that you want to disable.
        /// Disabled wallet won't be shown to the user as local connect options.
        /// </summary>
        /// <remarks>
        /// Use either <see cref="EnabledWallets"/> or <see cref="DisabledWallets"/>.
        /// If both are provided <see cref="DisabledWallets"/> will be ignored.
        /// </remarks>
        IList<string>? DisabledWallets { get; }

        /// <summary>
        /// Use this to set Local, Remote or Both wallet connection options for the user.
        /// </summary>
        WalletLocationOption WalletLocationOption { get; }

        /// <summary>
        /// Provider of the connection handling strategy.
        /// When user connects new session he has to choose which wallet he wants to use.
        /// This property is responsible for this task.
        /// </summary>
        IConnectionHandlerProvider ConnectionHandlerProvider { get; }

        /// <summary>
        /// Override for the registry URI used to download the list of wallets supported by WalletConnect.
        /// </summary>
        string? OverrideRegistryUri { get; }

        WalletConnectLogLevel LogLevel { get; }
    }
}