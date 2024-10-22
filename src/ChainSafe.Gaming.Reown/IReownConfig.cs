#nullable enable
using System.Collections.Generic;
using ChainSafe.Gaming.Reown.Connection;
using ChainSafe.Gaming.Reown.Wallets;
using ChainSafe.Gaming.Web3.Environment;
using ChainSafe.Gaming.Web3.Evm.Wallet;
using Reown.Core;
using Reown.Core.Network;
using Reown.Core.Network.Interfaces;

namespace ChainSafe.Gaming.Reown
{
    /// <summary>
    /// Interface for Reown configuration.
    /// </summary>
    public interface IReownConfig : IWalletProviderConfig
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
        /// Project ID provided to you by Reown.
        /// </summary>
        string ProjectId { get; }

        /// <summary>
        /// Name of the context to be used by Reown.
        /// </summary>
        string BaseContext { get; }

        /// <summary>
        /// Reown metadata.
        /// </summary>
        Metadata Metadata { get; }

        /// <summary>
        /// List of the wallets that you want to show to your user as local wallet connect options.
        /// Keep this empty to enable all of the wallets supported for the platform.
        /// </summary>
        IList<string>? IncludeWalletIds { get; }

        /// <summary>
        /// List of the wallets that you want to disable.
        /// Disabled wallet won't be shown to the user as local connect option.
        /// </summary>
        IList<string>? ExcludeWalletIds { get; }

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
        /// Override for the registry URI used to download the list of wallets supported by Reown.
        /// </summary>
        string? OverrideRegistryUri { get; }

        /// <summary>
        /// Configure log level filter for Reown logs.
        /// </summary>
        ReownLogLevel LogLevel { get; }

        /// <summary>
        /// I honestly don't know what this is for.
        /// </summary>
        IRelayUrlBuilder RelayUrlBuilder { get; }

        /// <summary>
        /// Used to set custom <see cref="IConnectionBuilder"/> to support Unity versions before 2022.1.
        /// </summary>
        IConnectionBuilder ConnectionBuilder { get; }
    }
}