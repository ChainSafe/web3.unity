#define RAMP_AVAILABLE

using System;
using System.Threading.Tasks;
using ChainSafe.Gaming.UnityPackage;
using ChainSafe.Gaming.UnityPackage.UI;
using UnityEngine;

namespace ChainSafe.Gaming.Exchangers.Ramp.Sample
{
    public class RampSample : MonoBehaviour, ISample
    {
        [field: SerializeField] public string Title { get; private set; }
        [field: SerializeField, TextArea] public string Description { get; private set; }

        public Type[] DependentServiceTypes => new[] { typeof(IRampExchanger) };

        public async Task<string> OnRamp()
        {
            LoadingOverlay.HideLoadingOverlay();
            // Show "Buy Crypto" widget
            var purchaseData = await Web3Unity.Web3.RampExchanger().BuyCrypto(
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

            return $"Purchase request: {purchaseData}";
        }

        public async Task<string> OffRamp()
        {
            LoadingOverlay.HideLoadingOverlay();

            // Show "Sell Crypto" widget
            var saleData = await Web3Unity.Web3.RampExchanger().SellCrypto(
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

            return $"OffRamp: {saleData}";
        }

        public async Task<string> OnRampOffRamp()
        {
            LoadingOverlay.HideLoadingOverlay();

            // Show "Buy or Sell Crypto" widget
            var rampTransactionData = await Web3Unity.Web3.RampExchanger().BuyOrSellCrypto(
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

            return rampTransactionData.ToString();
        }
    }
}
