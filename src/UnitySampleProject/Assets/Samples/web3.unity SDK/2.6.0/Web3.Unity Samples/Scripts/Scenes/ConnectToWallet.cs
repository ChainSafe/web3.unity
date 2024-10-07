using System;
using System.Threading.Tasks;
using ChainSafe.Gaming;
using ChainSafe.Gaming.UnityPackage;
using ChainSafe.Gaming.UnityPackage.Connection;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Core.Logout;
using Microsoft.Extensions.DependencyInjection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Used to easily connect/disconnect a wallet.
/// </summary>
public class ConnectToWallet : Web3BuilderServiceAdapter, IWeb3InitializedHandler, ILogoutHandler
{
    [SerializeField] private bool connectOnInitialize = true;
    
    [Space]
    
    [SerializeField] private Button connectButton;
    
    [SerializeField] private Button disconnectButton;
    
    [Space]
    
    [SerializeField] private TextMeshProUGUI addressText;

    [SerializeField] private Button copyAddressButton;
    
    [Space]
    
    [SerializeField] private Transform connectedTransform;
    
    [SerializeField] private Transform disconnectedTransform;
    
    private async void Start()
    {
        try
        {
            await Web3Unity.Instance.Initialize(connectOnInitialize);
        }
        finally
        {
            AddButtonListeners();
            
            ConnectionStateChanged(Web3Unity.Connected, Web3Unity.Instance.Address);
        }
    }

    private void AddButtonListeners()
    {
        connectButton.onClick.AddListener(Web3Unity.ConnectScreen.Open);
        
        disconnectButton.onClick.AddListener(Disconnect);
        
        copyAddressButton.onClick.AddListener(CopyAddress);

        void CopyAddress()
        {
            ClipboardManager.CopyText(addressText.text);
        }
    }
    
    private void ConnectionStateChanged(bool connected, string address = "")
    {
        connectedTransform.gameObject.SetActive(connected);
        
        disconnectedTransform.gameObject.SetActive(!connected);

        if (connected)
        {
            addressText.text = address;
        }
    }
    
    public Task OnWeb3Initialized(Web3 web3)
    {
        ConnectionStateChanged(true, web3.Signer.PublicAddress);
        
        return Task.CompletedTask;
    }

    private async void Disconnect()
    {
        await Web3Unity.Instance.Disconnect();
    }
    
    public Task OnLogout()
    {
        ConnectionStateChanged(false);
        
        return Task.CompletedTask;
    }

    public override Web3Builder ConfigureServices(Web3Builder web3Builder)
    {
        return web3Builder.Configure(services =>
        {
            services.AddSingleton<ILogoutHandler, IWeb3InitializedHandler, ConnectToWallet>(_ => this);
        });
    }
}
