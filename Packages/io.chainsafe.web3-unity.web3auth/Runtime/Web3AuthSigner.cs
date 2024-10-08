using System.Threading.Tasks;
using ChainSafe.Gaming.InProcessSigner;
using ChainSafe.Gaming.Web3.Core;
using ChainSafe.Gaming.Web3.Core.Logout;
using ChainSafe.Gaming.Web3.Evm.Wallet;

/// <summary>
/// Signs using a Web3Auth wallet.
/// </summary>
public class Web3AuthSigner : InProcessSigner, ILifecycleParticipant, ILogoutHandler
{
    private readonly IWalletProvider _walletProvider;

    public Web3AuthSigner(IAccountProvider accountProvider, IWalletProvider walletProvider) : base(accountProvider)
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
