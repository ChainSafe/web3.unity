using ChainSafe.Gaming.Evm.JsonRpc;
using ChainSafe.Gaming.UnityPackage;
using ChainSafe.Gaming.Wallets;
using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Unity;
using UnityEngine;

namespace ChainSafe.Gaming.Exchangers.Ramp
{
    public class RampExchangerSample : MonoBehaviour
    {
        private RampExchangerConfigScriptableObject RampConfig;
        
        private Web3.Web3 web3;
        
        private async void Awake()
        {
            web3 = await new Web3Builder(ProjectConfigUtilities.Load())
                .Configure(services =>
                {
                    services.UseUnityEnvironment();
                    services.UseRpcProvider();
                    services.UseWebPageWallet();
                    services.UseRampExchanger(RampConfig);
                })
                .BuildAsync();
        }

        public async void OnOnRampPressed()
        {
            var purchaseData = await web3.RampExchanger().BuyCrypto(
                new RampBuyWidgetSettings
                {
                    // For more info on widget settings check https://docs.ramp.network/configuration
                    SwapAsset = "ETH_ETH"
                });
            
            Debug.Log($"Success! Response: {purchaseData}");
        }

        public void OnOffRampPressed()
        {
            // todo
        }

        public void OnBothPressed()
        {
            // todo
        }
    }
}