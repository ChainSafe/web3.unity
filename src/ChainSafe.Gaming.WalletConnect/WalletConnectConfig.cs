using System;
using System.Collections.Generic;
using ChainSafe.Gaming.WalletConnect.Models;
using Newtonsoft.Json;
using WalletConnectSharp.Core;
using WalletConnectSharp.Sign.Models;
using WalletConnectSharp.Sign.Models.Engine;

namespace ChainSafe.Gaming.WalletConnect
{
    /// <summary>
    /// Used for configuring a wallet connect session.
    /// </summary>
    /// <remarks>Serialize fields explicitly or only ones with [JsonProperty] attribute used for restoring/renewing a session.</remarks>
    [Serializable]
    [JsonObject(MemberSerialization.OptIn)]
    public class WalletConnectConfig
    {
        /// <summary>
        /// Delegate used for when a session to connect to a wallet is initialized.
        /// </summary>
        public delegate void Connected(ConnectedData connectedData);

        /// <summary>
        /// Delegate used for when a connection session is approved from wallet.
        /// </summary>
        public delegate void SessionApproved(SessionStruct session);

        /// <summary>
        /// Event that's triggered when a session to connect to a wallet is initialized.
        /// </summary>
        public event Connected OnConnected;

        /// <summary>
        /// Event triggered when a connection session is approved from wallet.
        /// </summary>
        public event SessionApproved OnSessionApproved;

        /// <summary>
        /// Wallet Connect Project Id, can be found in Wallet Connect web dashboard.
        /// </summary>
        public string ProjectId { get; set; }

        /// <summary>
        /// Wallet Connect Project Name.
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// Topic for any saved Wallet Connect sessions.
        /// Used for restoring already existing sessions.
        /// </summary>
        [JsonProperty]
        public string SavedSessionTopic { get; set; }

        /// <summary>
        /// Storage directory for Wallet Connect json file.
        /// This file is generally used for restoring sessions.
        /// </summary>
        public string StoragePath { get; set; }

        /// <summary>
        /// Wallet Connect platform and usage context.
        /// For example "unity-game" is used in a Unity Made Game.
        /// </summary>
        public string BaseContext { get; set; }

        /// <summary>
        /// Chain of wallet to connect to, eg - Goerli.
        /// Used for specifying required namespaces for connecting to wallet.
        /// </summary>
        public ChainModel Chain { get; set; }

        /// <summary>
        /// Metadata for dapp/project.
        /// This will be displayed to wallet user when connecting and on other wallet connect prompts.
        /// </summary>
        public Metadata Metadata { get; set; }

        /// <summary>
        /// If wallet is installed on the same device as dapp should it redirect to wallet for any and all prompts.
        /// This uses deeplink-ing and might not work and/or be available for every wallet and/or platform.
        /// </summary>
        [JsonProperty]
        public bool RedirectToWallet { get; set; }

        /// <summary>
        /// Keep renewing session when expired.
        /// </summary>
        [JsonProperty]
        public bool KeepSessionAlive { get; set; }

        /// <summary>
        /// Default wallet that's being used, or a session is connected to.
        /// This works together with <see cref="RedirectToWallet"/>.
        /// </summary>
        [JsonProperty]
        public WalletConnectWalletModel DefaultWallet { get; set; }

        /// <summary>
        /// All wallets supported by Wallet Connect, see https://registry.walletconnect.org/data/wallets.json.
        /// </summary>
        public Dictionary<string, WalletConnectWalletModel> SupportedWallets { get; set; }

        /// <summary>
        /// Set to true if running tests.
        /// </summary>
        public bool Testing { get; set; } = false;

        /// <summary>
        /// Public Address used for testing.
        /// This is only relevant if <see cref="Testing"/> is set to true.
        /// </summary>
        public string TestWalletAddress { get; set; } = null;

        /// <summary>
        /// if <see cref="Testing"/> is set to true replace this with already made response hash before making the request.
        /// This is used to skip required approval from a user when testing.
        /// </summary>
        public string TestResponse { get; set; } = string.Empty;

        internal void InvokeConnected(ConnectedData connectedData)
        {
            OnConnected?.Invoke(connectedData);
        }

        internal void InvokeSessionApproved(SessionStruct session)
        {
            OnSessionApproved?.Invoke(session);
        }
    }
}