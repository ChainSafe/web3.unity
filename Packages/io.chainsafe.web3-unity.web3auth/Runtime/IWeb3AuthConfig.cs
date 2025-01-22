using System;
using System.Threading;
using System.Threading.Tasks;
using ChainSafe.Gaming.EmbeddedWallet;
using Network = Web3Auth.Network;
using ThemeModes = Web3Auth.ThemeModes;
using Language = Web3Auth.Language;

namespace ChainSafe.GamingSdk.Web3Auth
{
    /// <summary>
    /// Configuration options for initializing a Web3AuthWallet instance.
    /// </summary>
    public interface IWeb3AuthConfig : IEmbeddedWalletConfig
    {
        // Name of the App.
        public string AppName { get; }
        // Client ID you get from Web3Auth dashboard.
        public string ClientId { get; }
        // Redirect URI for the app.
        public string RedirectUri { get; }
        // Network to connect to (MainNet, TestNet...).
        public Network Network { get; }
        public ThemeModes Theme { get; }
        public Language Language { get; }
        
        /// <summary>
        /// Gets or sets the Web3AuthOptions for configuring the Web3Auth instance associated with the wallet.
        /// </summary>
        public Web3AuthOptions Web3AuthOptions => new Web3AuthOptions
        {
            clientId = ClientId,
            redirectUrl = new Uri(RedirectUri),
            network = Network,
            
            whiteLabel = new()
            {
                mode = Theme,
                defaultLanguage = Language,
                appName = AppName,
            }
        };

        /// <summary>
        /// Login Provider to use when connecting the wallet, like Google, facebook etc...
        /// </summary>
        public Task<Provider> ProviderTask { get; }

        /// <summary>
        /// Get the SessionId on connection from the provider.
        /// </summary>
        public Task<string> SessionTask { get; }

        /// <summary>
        /// Token for cancelling a connection
        /// </summary>
        public CancellationToken CancellationToken { get; }
        
        /// <summary>
        /// Remember this session
        /// </summary>
        public bool RememberMe { get; }

        /// <summary>
        /// Remember a previous session and login automatically
        /// </summary>
        public bool AutoLogin { get; }
    }
}
