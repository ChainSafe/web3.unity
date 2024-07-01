using ChainSafe.Gaming.Web3.Evm.Wallet;
using WalletConnectUnity.Modal;

namespace ChainSafe.Gaming.WalletConnectUnity
{
    public interface IWalletConnectUnityConfig : IWalletProviderConfig
    {
        // public bool ShouldSpawnModal { get; }
        //
        // public WalletConnectModal ModalPrefab { get; }
        
        public string[] IncludedWalletIds { get; }

        public string[] ExcludedWalletIds { get; }
    }
}