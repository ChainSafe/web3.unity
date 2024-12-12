#if APPKIT_AVAILABLE
using System;
using ChainSafe.Gaming.Reown.AppKit;
using ChainSafe.Gaming.UnityPackage.Connection;
using ChainSafe.Gaming.Web3.Build;
using UnityEngine;

public class AppKitServiceAdapter : ServiceAdapter
{
    public override Web3Builder ConfigureServices(Web3Builder web3Builder)
    {
        var reownConnectionProvider = Resources.Load<ReownConnectionProvider>("ReownConnectionProvider");
        if (reownConnectionProvider == null)
            throw new InvalidOperationException("ReownConnectionProvider not found in Resources folder");
        web3Builder.Configure(x =>
        {
            x.UseAppKit(reownConnectionProvider);
            x.UseAppKitSigner();
            x.UseAppKitTransactionExecutor();
        });
        return web3Builder;
    }
}
#endif