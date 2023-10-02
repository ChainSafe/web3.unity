using System;
using Newtonsoft.Json;
using WalletConnectSharp.Sign.Models.Engine;

namespace ChainSafe.Gaming.WalletConnect
{
    public abstract class WalletConnectWalletModel
    {
        [JsonProperty("mobile")]
        public WalletLink Mobile { get; private set; }

        [JsonProperty("desktop")]
        public WalletLink Desktop { get; private set; }

        public abstract void OpenDeeplink(ConnectedData data, bool useNative = false);

        public abstract void OpenWallet();
    }
}