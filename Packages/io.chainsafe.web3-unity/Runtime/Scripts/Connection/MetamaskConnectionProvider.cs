using System.Threading.Tasks;
using ChainSafe.Gaming.Unity.MetaMask;
using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Evm.Wallet;
using UnityEngine;

namespace ChainSafe.Gaming.UnityPackage.Connection
{
    /// <summary>
    /// Metamask connection provider used for connecting to a Metamask wallet.
    /// </summary>
    public class MetamaskConnectionProvider : ConnectionProvider
    {
        public override bool IsAvailable => Application.platform == RuntimePlatform.WebGLPlayer && Application.isEditor == false;

        public override Task Initialize()
        {
            return Task.CompletedTask;
        }

        public override Web3Builder ConfigureServices(Web3Builder web3Builder)
        {
            return web3Builder.Configure(services =>
            {
                services.UseMetaMask().UseWalletSigner().UseWalletTransactionExecutor();
            });
        }
    }
}
