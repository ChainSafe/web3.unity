using System.Threading.Tasks;
using ChainSafe.Gaming.Web3.Core.Debug;
using ChainSafe.Gaming.Web3.Evm.Wallet;

namespace ChainSafe.Gaming.Reown.AppKit
{
    /// <summary>
    /// AppKitSigner is different from the default WaleltSigner because AppKitSigner already requests network switching
    /// in webgl if the wallet itself is not on the correct network.
    /// </summary>
    public class AppKitSigner : WalletSigner
    {
        public AppKitSigner(IWalletProvider walletProvider, IWalletProviderConfig walletConfig) : base(walletProvider, walletConfig)
        {
        }

        public override async ValueTask WillStartAsync()
        {
            string address = await WalletProvider.Connect();
            PublicAddress = address.AssertIsPublicAddress();
        }
    }
}