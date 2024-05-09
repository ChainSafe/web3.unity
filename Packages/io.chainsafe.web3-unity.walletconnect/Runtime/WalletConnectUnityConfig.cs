using System;

namespace ChainSafe.Gaming.WalletConnectUnity
{
    [Serializable]
    public class WalletConnectUnityConfig : IWalletConnectUnityConfig
    {
        public string[] IncludedWalletIds { get; set; }
        
        public string[] ExcludedWalletIds { get; set; }
    }
}