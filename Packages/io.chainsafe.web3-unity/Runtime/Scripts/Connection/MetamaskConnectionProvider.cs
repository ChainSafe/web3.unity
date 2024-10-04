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
        [field: SerializeField, DefaultAssetValue("Packages/io.chainsafe.web3-unity/Runtime/Prefabs/MetamaskRow.prefab")]
        public override Button ConnectButtonRow { get; protected set; }

        public override bool IsAvailable => Application.platform == RuntimePlatform.WebGLPlayer && Application.isEditor == false;

        public override Task Initialize(bool rememberSession)
        {
            return Task.CompletedTask;
        }

        protected override void ConfigureServices(IWeb3ServiceCollection services)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            services.UseMetaMask().UseWalletSigner().UseWalletTransactionExecutor();
#endif
        }

        public override Task<bool> SavedSessionAvailable()
        {
            return Task.FromResult(false);
        }
    }
}
