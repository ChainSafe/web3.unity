using ChainSafe.Gaming.Evm.JsonRpc;
using ChainSafe.Gaming.Exchangers.Ramp;
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
    public RampExchangerConfigScriptableObject RampConfig;
    public WalletConnectConfig WalletConnectConfig;
    public Button OnRampButton;
    public Button OffRampButton;
    public Button OnRampOffRampButton;

    private Web3 web3;
    
    private async void Awake()
    {
        // Build Web3
        web3 = await new Web3Builder(ProjectConfigUtilities.Load())
            .Configure(services =>
            {
                services.UseUnityEnvironment();
                services.UseRpcProvider();
                UsePlatformSpecificWallet();
                services.UseRampExchanger(RampConfig);

                void UsePlatformSpecificWallet()
                {
                    if (Application.platform != RuntimePlatform.WebGLPlayer)
                        services.UseWalletConnect(WalletConnectConfig);
                    else
                        services.UseWebGLWallet();
                }
            })
            .LaunchAsync();
        
        // Subscribe to buttons
        OnRampButton.onClick.AddListener(OnRampPressed);
        OffRampButton.onClick.AddListener(OffRampPressed);
        OnRampOffRampButton.onClick.AddListener(OnRampOffRampPressed);
    }

    private async void OnRampPressed()
    {
        // Show "Buy Crypto" widget
        var purchaseData = await web3.RampExchanger().BuyCrypto(
            new RampBuyWidgetSettings
            {
                // For more info on widget settings check https://docs.ramp.network/configuration
                SwapAsset = "ETH_DAI,ETH_ETH,ETH_USDC",
                DefaultAsset = "ETH",
                FiatCurrency = "UAH",
                FiatValue = 100,
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
                OfframpAsset = "ETH_ETH"
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
                SwapAsset = "ETH_DAI,ETH_ETH",
                OfframpAsset = "ETH_ETH,ETH_USDC",
                DefaultAsset = "ETH_ETH",
                FiatCurrency = "CAD",
                FiatValue = 100,
                UserEmailAddress = "test@test.com",
                SwapAmount = 5,
                SelectedCountryCode = "CA"
            });
        
        Debug.Log(rampTransactionData.ToString());
    }
}
