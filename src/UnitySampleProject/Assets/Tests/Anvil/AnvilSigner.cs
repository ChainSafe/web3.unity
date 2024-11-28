using System.Threading.Tasks;
using ChainSafe.Gaming.InProcessSigner;
using ChainSafe.Gaming.Web3.Core;
using ChainSafe.Gaming.Web3.Core.Logout;
using ChainSafe.Gaming.Web3.Evm.Wallet;

namespace ChainSafe.Gaming.Unity.Tests
{
    public class AnvilSigner : InProcessSigner.InProcessSigner, ILifecycleParticipant, ILogoutHandler
    {
        private readonly IWalletProvider _walletProvider;

        public AnvilSigner(IAccountProvider accountProvider, IWalletProvider walletProvider) : base(accountProvider)
        {
            _walletProvider = walletProvider;
        }

        public async ValueTask WillStartAsync()
        {
            await _walletProvider.Connect();
        }

        public virtual ValueTask WillStopAsync()
        {
            return new ValueTask(Task.CompletedTask);
        }

        public async Task OnLogout()
        {
            await _walletProvider.Disconnect();
        }
    }
}
