using System;

namespace ChainSafe.GamingSDK.EVM.MetaMaskBrowserWallet
{
    [Serializable]
    public class MetaMaskBrowserSignerConfiguration
    {
        public string ServiceUrl { get; set; } = "https://chainsafe.github.io/game-web3wallet/";

        public TimeSpan ClipboardCheckPeriod { get; set; } = TimeSpan.FromMilliseconds(100);
    }
}