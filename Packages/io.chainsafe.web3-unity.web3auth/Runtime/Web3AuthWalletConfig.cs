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
        /// Gets or sets the login parameters for authenticating with the Web3Auth instance. These parameters
        /// may include authentication credentials or other required data.
        /// </summary>
        public LoginParams LoginParams { get; set; }
    }
}
