using System;

namespace ChainSafe.GamingSDK.EVM.MetaMaskBrowserWallet
{
    [Serializable]
    public class WebPageWalletConfig
    {
        public string ServiceUrl { get; set; } = "https://chainsafe.github.io/game-web3wallet/";

        public TimeSpan ClipboardCheckPeriod { get; set; } = TimeSpan.FromMilliseconds(100);
    }
}