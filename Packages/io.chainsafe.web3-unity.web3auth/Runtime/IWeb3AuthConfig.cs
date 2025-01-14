using System.Threading;
using System.Threading.Tasks;
using ChainSafe.Gaming.EmbeddedWallet;
using TWeb3Auth = Web3Auth;

namespace ChainSafe.GamingSdk.Web3Auth
{
    /// <summary>
    /// Configuration options for initializing a Web3AuthWallet instance.
    /// </summary>
    public interface IWeb3AuthConfig : IEmbeddedWalletConfig
    {
        /// <summary>
        /// Gets or sets the Web3AuthOptions for configuring the Web3Auth instance associated with the wallet.
        /// </summary>
        public Web3AuthOptions Web3AuthOptions { get; }

        /// <summary>
        /// Login Provider to use when connecting the wallet, like Google, facebook etc...
        /// </summary>
        public Task<Provider> ProviderTask { get; }

        /// <summary>
        /// Get the SessionId on connection from the provider.
        /// </summary>
        public Task<string> SessionTask { get; set; }

        public CancellationToken CancellationToken { get; }
        
        public bool RememberMe { get; }

        public bool AutoLogin { get; }
    }
}
