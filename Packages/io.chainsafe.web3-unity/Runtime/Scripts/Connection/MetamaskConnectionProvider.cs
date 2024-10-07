using System.Threading.Tasks;
#if UNITY_WEBGL && !UNITY_EDITOR
using ChainSafe.Gaming.Unity.MetaMask;
using ChainSafe.Gaming.Web3.Evm.Wallet;
#endif
using ChainSafe.Gaming.Web3.Build;
using UnityEngine;
using UnityEngine.UI;

namespace ChainSafe.Gaming.UnityPackage.Connection
{
    /// <summary>
    /// Metamask's connection provider used for connecting to a Metamask wallet.
    /// </summary>
    [CreateAssetMenu(menuName = "ChainSafe/Connection Provider/Metamask", fileName = nameof(MetamaskConnectionProvider))]
    public class MetamaskConnectionProvider : ConnectionProvider
    {
        [field: SerializeField, DefaultAssetValue("Packages/io.chainsafe.web3-unity/Runtime/Sprites/MetaMask_Icon.png")]
        public override Sprite ButtonIcon { get; protected set; }

        [field: SerializeField] public override string ButtonText { get; protected set; } = "MetaMask";

        public override bool IsAvailable => Application.platform == RuntimePlatform.WebGLPlayer && Application.isEditor == false;

        public override Task Initialize()
        {
            return Task.CompletedTask;
        }

        public override Web3Builder ConfigureServices(Web3Builder web3Builder)
        {
            return web3Builder.Configure(services =>
            {
#if UNITY_WEBGL && !UNITY_EDITOR
                services.UseMetaMask().UseWalletSigner().UseWalletTransactionExecutor();
#endif
            });
        }
    }
}
