using System;
using ChainSafe.Gaming.Evm.JsonRpc;
using ChainSafe.Gaming.Exchangers.Ramp;
using ChainSafe.Gaming.UnityPackage;
using ChainSafe.Gaming.WalletConnect;
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
    public WalletConnectConfig WalletConnectConfig;
    
    private Web3 web3;
    
    private async void Awake()
    {
        OnRampButton.onClick.AddListener(OnRampPressed);
        OffRampButton.onClick.AddListener(OffRampPressed);
        OnRampOffRampButton.onClick.AddListener(OnRampOffRampPressed);

        web3 = await new Web3Builder(ProjectConfigUtilities.Load())
            .Configure(services =>
            {
                services.UseUnityEnvironment();
                services.UseRpcProvider();
                services.UseWalletConnect(WalletConnectConfig);
                services.UseRampExchanger(new RampExchangerConfig
                {
                    // todo
                });
            })
            .LaunchAsync();
    }

    private async void OnRampPressed()
    {
        var purchaseData = await web3.RampExchanger().BuyCrypto(
            new RampBuyWidgetSettings
            {
                // For more info on widget settings check https://docs.ramp.network/configuration
                SwapAsset = "ETH_ETH"
            });
        
        Debug.Log($"OnRamp success! Response: {purchaseData}");
    }

    private async void OffRampPressed()
    {
        var purchaseData = await web3.RampExchanger().SellCrypto(
            new RampSellWidgetSettings
            {
                // For more info on widget settings check https://docs.ramp.network/configuration 
                OfframpAsset = "ETH_ETH"
            });
        
        Debug.Log($"OffRamp success! Response: {purchaseData}");
    }

    private void OnRampOffRampPressed()
    {
        throw new NotImplementedException();
    }
}
