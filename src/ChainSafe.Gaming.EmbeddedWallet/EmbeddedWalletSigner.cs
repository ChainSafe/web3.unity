using System.Threading.Tasks;
using ChainSafe.Gaming.InProcessSigner;
using ChainSafe.Gaming.Web3.Core;
using ChainSafe.Gaming.Web3.Core.Logout;
using ChainSafe.Gaming.Web3.Evm.Wallet;
using Signer = ChainSafe.Gaming.InProcessSigner.InProcessSigner;

namespace ChainSafe.Gaming.EmbeddedWallet
{
    public class EmbeddedWalletSigner : Signer, ILifecycleParticipant, ILogoutHandler
    {
        private readonly IWalletProvider walletProvider;

        public EmbeddedWalletSigner(IAccountProvider accountProvider, IWalletProvider walletProvider)
            : base(accountProvider)
        {
            this.walletProvider = walletProvider;
        }

        public async ValueTask WillStartAsync()
        {
            await walletProvider.Connect();
        }

        public virtual ValueTask WillStopAsync()
        {
            return new ValueTask(Task.CompletedTask);
        }

        public async Task OnLogout()
        {
            await walletProvider.Disconnect();
        }
    }
}