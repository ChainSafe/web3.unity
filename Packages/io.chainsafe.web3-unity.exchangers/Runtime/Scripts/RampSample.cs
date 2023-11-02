using System;
using ChainSafe.Gaming.Evm.JsonRpc;
using ChainSafe.Gaming.Exchangers.Ramp;
using ChainSafe.Gaming.UnityPackage;
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
        OnRampButton.onClick.AddListener(OnRampPressed);
        OffRampButton.onClick.AddListener(OffRampPressed);
        OnRampOffRampButton.onClick.AddListener(OnRampOffRampPressed);
        
        web3 = await new Web3Builder(ProjectConfigUtilities.Load())
            .Configure(services =>
            {
                services.UseUnityEnvironment();
                services.UseRpcProvider();
                services.UseWebPageWallet();
                services.UseRampExchanger(new RampExchangerConfig
                {
                    // todo
                });
            })
            .BuildAsync();
    }

    private async void OnRampPressed()
    {
        var purchaseData = await web3.RampExchanger().BuyCrypto(new RampBuyWidgetSettings
        {
            // todo
        });
        
        // todo log
    }

    private void OffRampPressed()
    {
        throw new NotImplementedException();
    }

    private void OnRampOffRampPressed()
    {
        throw new NotImplementedException();
    }
}
