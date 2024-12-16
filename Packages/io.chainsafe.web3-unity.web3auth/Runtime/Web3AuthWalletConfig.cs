using System.Threading;
using System.Threading.Tasks;
using TWeb3Auth = Web3Auth;

namespace ChainSafe.GamingSdk.Web3Auth
{
    /// <summary>
    /// Configuration options for initializing a Web3AuthWallet instance.
    /// </summary>
    public class Web3AuthWalletConfig
    {
        /// <summary>
        /// Gets or sets the Web3AuthOptions for configuring the Web3Auth instance associated with the wallet.
        /// </summary>
        public Web3AuthOptions Web3AuthOptions { get; set; }

        /// <summary>
        /// Login Provider to use when connecting the wallet, like Google, facebook etc...
        /// </summary>
        public Task<Provider> ProviderTask { get; set; }

        /// <summary>
        /// Get the SessionId on connection from the provider.
        /// </summary>
        public Task<string> SessionTask { get; set; }

        public CancellationToken CancellationToken { get; set; }

        public bool RememberMe { get; set; }

        public bool AutoLogin { get; set; }
        
        public bool UseWalletGui { get; set; }
    }
}
