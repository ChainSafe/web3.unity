using System;
using ChainSafe.Gaming.WalletConnect;

namespace ChainSafe.Gaming.WalletConnect
{
    [Serializable]
    public class WebPageWalletConfig
    {
        public string SavedUserAddress { get; set; } = null;

        public string ServiceUrl { get; set; } = "https://chainsafe.github.io/game-web3wallet/";

        public TimeSpan ClipboardCheckPeriod { get; set; } = TimeSpan.FromMilliseconds(100);

        public TimeSpan ConnectRequestExpiresAfter { get; set; } = TimeSpan.FromMinutes(1);

        public WalletConnectWallet.ConnectMessageBuildDelegate ConnectMessageBuilder { get; set; } =
            time => $"Sign this message to connect your account. This request will expire at {time:hh:mm:ss}.";

        public WalletConnectConfig WalletConnectConfig { get; set; }
    }
}