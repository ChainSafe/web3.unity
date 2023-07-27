using TWeb3Auth = Web3Auth;

namespace ChainSafe.GamingSdk.Web3Auth
{
    public class Web3AuthWalletConfig
    {
        // Since Web3Auth is a Unity component, we must receive it from
        // outside as we are running without access to any Unity objects
        // in the wallet code.
        public TWeb3Auth Web3Auth { get; set; }

        public Web3AuthOptions Web3AuthOptions { get; set; }

        public LoginParams LoginParams { get; set; }
    }
}
