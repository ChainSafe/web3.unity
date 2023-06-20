using System;

namespace ChainSafe.GamingSDK.EVM.MetaMaskBrowserWallet
{
    [Serializable]
    public class WebPageWalletConfig
    {
        public string ServiceUrl { get; set; } = "https://chainsafe.github.io/game-web3wallet/";

        public TimeSpan ClipboardCheckPeriod { get; set; } = TimeSpan.FromMilliseconds(100);

        public TimeSpan ConnectRequestExpiresAfter { get; set; } = TimeSpan.FromMinutes(1);

        public WebPageWallet.ConnectMessageBuildDelegate ConnectMessageBuilder { get; set; } =
            time => $"Sign this message before {time:hh:mm:ss} to connect your account.";
    }
}