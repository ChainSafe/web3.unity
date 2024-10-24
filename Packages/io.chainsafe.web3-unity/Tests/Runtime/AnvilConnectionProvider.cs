#if UNITY_EDITOR
using System;
using System.Threading.Tasks;
using ChainSafe.Gaming.UnityPackage;
using ChainSafe.Gaming.UnityPackage.Connection;
using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Evm.Wallet;
using UnityEngine.UI;

public class AnvilConnectionProvider : ConnectionProvider
{
    public override bool IsAvailable => Web3Unity.TestMode;
    public override Button ConnectButtonRow { get; protected set; }
    protected override void ConfigureServices(IWeb3ServiceCollection services)
    {
        services.UseWalletProvider<AnvilProvider>(new AnvilConfig()).UseWalletSigner().UseWalletTransactionExecutor();
    }

    public override Task<bool> SavedSessionAvailable()
    {
        return Task.FromResult(false);
    }
}

#endif