#define RAMP_AVAILABLE

using ChainSafe.Gaming.UnityPackage;
using ChainSafe.Gaming.Web3.Build;
using UnityEngine;
using UnityEngine.UI;

namespace ChainSafe.Gaming.Exchangers.Ramp.Sample
{
    public class RampSample : SampleBase<RampSample>
    {
        private async void OnRamp()
        {
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

            Debug.Log($"Purchase request: {purchaseData}");
        }

        private async void OffRamp()
        {
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

            Debug.Log($"OffRamp: {saleData}");
        }

        private async void OnRampOffRamp()
        {
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

            Debug.Log(rampTransactionData.ToString());
        }

        public override Web3Builder ConfigureServices(Web3Builder web3Builder)
        {
            return web3Builder;
        }
    }
}
