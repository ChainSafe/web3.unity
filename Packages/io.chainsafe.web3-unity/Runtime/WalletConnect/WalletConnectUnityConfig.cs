using System;
using WalletConnectUnity.Modal;

namespace ChainSafe.Gaming.WalletConnectUnity
{
    [Serializable]
    public class WalletConnectUnityConfig : IWalletConnectUnityConfig
    {
        // public bool ShouldSpawnModal { get; set; }
        //
        // public WalletConnectModal ModalPrefab { get; set; }

        public string[] ProjectId { get; set; }
        public string[] IncludedWalletIds { get; set; }
        
        public string[] ExcludedWalletIds { get; set; }
    }
}