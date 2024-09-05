using System.Threading.Tasks;
using ChainSafe.Gaming.LocalStorage;
using ChainSafe.Gaming.UnityPackage;
using ChainSafe.Gaming.UnityPackage.Connection;
using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Evm.Wallet;
using UnityEngine;
using UnityEngine.UI;

namespace ChainSafe.Gaming.HyperPlay
{
    /// <summary>
    /// Connection provider for connecting via HyperPlay Launcher.
    /// </summary>
    [CreateAssetMenu(menuName = "ChainSafe/Connection Provider/HyperPlay", fileName = nameof(HyperPlayConnectionProvider))]
    public class HyperPlayConnectionProvider : RestorableConnectionProvider, IHyperPlayConfig
    {
        public string SignMessageRpcMethodName => "personal_sign";

        public string SignTypedMessageRpcMethodName => "eth_signTypedData_v3";
        
        [field: SerializeField, DefaultAssetValue("Packages/io.chainsafe.web3-unity.hyperplay/Runtime/Prefabs/HyperPlayRow.prefab")]
        public override Button ConnectButtonRow { get; protected set; }

        public bool RememberConnection => RememberSession;
        
        public override bool IsAvailable => Application.isEditor || !Application.isMobilePlatform;

        private bool _storedSessionAvailable;
        
        public override Task Initialize()
        {
            return Task.CompletedTask;
        }
        
        protected override void ConfigureServices(IWeb3ServiceCollection services)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            services.UseHyperPlay<HyperPlayWebGLProvider>(this);
#else
            services.UseHyperPlay(this);
#endif
            services.UseWalletSigner().UseWalletTransactionExecutor();
        }

        public override async Task<bool> SavedSessionAvailable()
        {
            var data = new HyperPlayData();

            await data.LoadOneTime();
            
            _storedSessionAvailable = data.RememberSession;

            return _storedSessionAvailable;
        }
    }
}
