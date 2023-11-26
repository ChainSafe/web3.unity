using ChainSafe.Gaming.Evm.JsonRpc;
using ChainSafe.Gaming.Exchangers.Ramp;
using ChainSafe.Gaming.InProcessSigner;
using ChainSafe.Gaming.UnityPackage;
using ChainSafe.Gaming.WalletConnect;
using ChainSafe.Gaming.Wallets;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Unity;
using UnityEngine;
using UnityEngine.UI;

// TODO move me to samples
public class RampSample : MonoBehaviour
{
    public Button OnRampButton;
    public Button OffRampButton;
    public Button OnRampOffRampButton;

    private Web3 web3;
    
    private async void Awake()
    {
        
        // Subscribe to buttons
        OnRampButton.onClick.AddListener(OnRampPressed);
        OffRampButton.onClick.AddListener(OffRampPressed);
        OnRampOffRampButton.onClick.AddListener(OnRampOffRampPressed);
        web3 = Web3Accessor.Web3;
    }

    private async void OnRampPressed()
    {
        Debug.Log("OnRampPressed");
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
        
        Debug.Log($"OnRamp success! Response: {purchaseData}");
    }

    private async void OffRampPressed()
    {
        // Show "Sell Crypto" widget
        var saleData = await web3.RampExchanger().SellCrypto(
            new RampSellWidgetSettings
            {
                // For more info on widget settings check https://docs.ramp.network/configuration 
                // For more info on widget settings check https://docs.ramp.network/configuration 
                OfframpAsset = "SEPOLIA_ETH",
                DefaultAsset = "SEPOLIA_ETH",
                FiatCurrency = "EUR",
                FiatValue = 100,
                UserEmailAddress = "test@test.com",
                SwapAmount = 5,
                SelectedCountryCode = "RS"            
            });
        
        Debug.Log($"OffRamp success! Response: {saleData}");
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
