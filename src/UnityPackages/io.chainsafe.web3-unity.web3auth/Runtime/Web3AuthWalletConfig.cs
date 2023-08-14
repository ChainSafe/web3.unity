using TWeb3Auth = Web3Auth;

namespace ChainSafe.GamingSdk.Web3Auth
{
    public class Web3AuthWalletConfig
    {
        public string ClientId { get; set; }

        public string RedirectUri { get; set; }

        public TWeb3Auth.Network Network { get; set; }

        public Web3AuthOptions Web3AuthOptions { get; set; }

        public LoginParams LoginParams { get; set; }
    }
}
