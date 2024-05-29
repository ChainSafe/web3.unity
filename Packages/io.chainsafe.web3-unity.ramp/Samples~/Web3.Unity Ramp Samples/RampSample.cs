#define RAMP_AVAILABLE
using ChainSafe.Gaming.Evm.JsonRpc;
using ChainSafe.Gaming.UnityPackage;
using ChainSafe.Gaming.Wallets;
using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Unity;
using UnityEngine;
using UnityEngine.UI;

namespace ChainSafe.Gaming.Exchangers.Ramp.Sample
{
    public class RampSample : MonoBehaviour
    {
        public RampExchangerConfigSO Config;
        public Button OnRampButton;
        public Button OffRampButton;
        public Button OnRampOffRampButton;

        private Web3.Web3 web3;
    
        private async void Awake()
        {
            // todo figure out how to handle login when other packages' samples were not loaded
            web3 = await new Web3Builder(ProjectConfigUtilities.Load())
                .Configure(services =>
                {
                    services.UseUnityEnvironment();
                    services.UseRpcProvider();
                    services.UseWebGLWallet(); // todo use WalletConnect for other platforms (for all platforms probably)
                    services.UseRampExchanger(Config);
                }).LaunchAsync();
        
            // Subscribe to buttons
            OnRampButton.onClick.AddListener(OnRampPressed);
            OffRampButton.onClick.AddListener(OffRampPressed);
            OnRampOffRampButton.onClick.AddListener(OnRampOffRampPressed);
        
            // Subscribe to Ramp events
            web3.RampExchanger().OnRampPurchaseCreated += data 
                => Debug.Log($"On-Ramp purchase created {data.CryptoAmount} {data.Asset.Name}");
            web3.RampExchanger().OffRampSaleCreated += data
                => Debug.Log($"Off-Ramp sale created {data.Fiat.Amount:C} {data.Fiat.CurrencySymbol}");
        }

        private async void OnRampPressed()
        {
            // Show "Buy Crypto" widget
            var purchaseData = await web3.RampExchanger().BuyCrypto(
                new RampBuyWidgetSettings
                {
                    // For more info on widget settings check https://docs.ramp.network/configuration
                    //Since this is only a demo, SEPOLIA_ETH is used as a fake token. For production environment,
                    //you should use real tokens like ETH, DAI, USDC, etc.
                    SwapAsset = "SEPOLIA_ETH",
                    DefaultAsset = "SEPOLIA_ETH",
                    FiatCurrency = "EUR",
                    FiatValue = 100,
                    UserEmailAddress = "test@test.com",
                    SwapAmount = 5,
                    SelectedCountryCode = "RS"
                });
        
            Debug.Log($"Purchase request: {purchaseData}");
        }

        private async void OffRampPressed()
        {
            // Show "Sell Crypto" widget
            var saleData = await web3.RampExchanger().SellCrypto(
                new RampSellWidgetSettings
                {
                    // For more info on widget settings check https://docs.ramp.network/configuration
                    OfframpAsset = "SEPOLIA_ETH",
                    DefaultAsset = "SEPOLIA_ETH",
                    FiatCurrency = "EUR",
                    FiatValue = 100,
                    UserEmailAddress = "test@test.com",
                    SwapAmount = 5,
                    SelectedCountryCode = "RS"            
                });
        
            Debug.Log($"OffRamp: {saleData}");
        }

        private async void OnRampOffRampPressed()
        {
            // Show "Buy or Sell Crypto" widget
            var rampTransactionData = await web3.RampExchanger().BuyOrSellCrypto(
                new RampBuyOrSellWidgetSettings
                {
                    // For more info on widget settings check https://docs.ramp.network/configuration 
                    SwapAsset = "SEPOLIA_ETH",
                    OfframpAsset = "SEPOLIA_ETH",
                    DefaultAsset = "SEPOLIA_ETH",
                    FiatCurrency = "EUR",
                    FiatValue = 100,
                    UserEmailAddress = "test@test.com",
                    SwapAmount = 5,
                    SelectedCountryCode = "RS"
                });
        
            Debug.Log(rampTransactionData.ToString());
        }
    }
}
