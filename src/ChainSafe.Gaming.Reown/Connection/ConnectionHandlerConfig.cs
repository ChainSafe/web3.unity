using System;
using System.Collections.Generic;
using ChainSafe.Gaming.Reown.Models;
using ChainSafe.Gaming.Reown.Wallets;
using ChainSafe.Gaming.Web3.Environment.Http;

namespace ChainSafe.Gaming.Reown.Connection
{
    /// <summary>
    /// The config object class used to configure the <see cref="IConnectionHandler"/>.
    /// </summary>
    public class ConnectionHandlerConfig
    {
        /// <summary>
        /// Used to determine which wallet connection options should be shown to the user.
        /// </summary>
        public WalletLocationOption WalletLocationOption { get; set; }

        /// <summary>
        /// Used to determine the list of wallets that can be used to connect using the same device the app is running in.
        /// </summary>
        public List<WalletModel> LocalWalletOptions { get; set; }

        /// <summary>
        /// HTTP Headers that can be used to access the Reown API.
        /// </summary>
        public HttpHeader[] HttpHeaders { get; set; }

        /// <summary>
        /// An endpoint that can be used to build a URI which points to the icon of the specified wallet.
        /// </summary>
        /// <remarks>Simply concatenate wallet image id to this string to get the final URI.</remarks>
        public string WalletIconEndpoint { get; set; }

        /// <summary>
        /// Used this delegate to redirect user to the desired wallet for connecting new session.
        /// </summary>
        public OpenLocalWalletDelegate RedirectToWallet { get; set; }

        /// <summary>
        /// Use this property to determine if OS supports wallet selection and you can delegate this task to the OS.
        /// </summary>
        public bool DelegateLocalWalletSelectionToOs { get; set; }

        /// <summary>
        /// URI used to connect wallet remotely. Usually used to generate a QR code.
        /// </summary>
        public string ConnectRemoteWalletUri { get; set; }

        /// <summary>
        /// Use this delegate to delegate wallet selection & redirection to OS.
        /// Only supported on Android currently.
        /// </summary>
        public Action RedirectOsManaged { get; set; }
    }
}