using ChainSafe.Gaming.Unity.MetaMask;
using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Evm.Wallet;
using UnityEngine;

namespace ChainSafe.Gaming
{
    public class MetamaskConnectionProvider : ConnectionProvider
    {
        public override bool IsAvailable => Application.platform == RuntimePlatform.WebGLPlayer && Application.isEditor == false;
        
        public override Web3Builder ConfigureServices(Web3Builder web3Builder)
        {
            return web3Builder.Configure(services =>
            {
                services.UseMetaMask().UseWalletSigner().UseWalletTransactionExecutor();
            });
        }
    }
}
