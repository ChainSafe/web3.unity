using System.Threading.Tasks;
using ChainSafe.Gaming.UnityPackage;
using ChainSafe.Gaming.UnityPackage.Connection;
using ChainSafe.Gaming.UnityPackage.UI;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Core.Logout;
using Microsoft.Extensions.DependencyInjection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Used to easily connect to wallet.
/// </summary>
public class ConnectToWallet : MonoBehaviour, IWeb3InitializedHandler, ILogoutHandler, IWeb3BuilderServiceAdapter
{
    public int Priority => 0;
    
    [SerializeField] private bool connectOnInitialize = true;
    
    [SerializeField] private Button connectButton;
    
    [SerializeField] private Button disconnectButton;
    
    [SerializeField] private TextMeshProUGUI addressText;

    [SerializeField] private Button copyAddressButton;
    
    private async void Start()
    {
        await Web3Unity.Instance.Initialize(connectOnInitialize);
        
        connectButton.onClick.AddListener(Web3Unity.ConnectModal.Open);
        
        disconnectButton.onClick.AddListener(Disconnect);
    }
    
    private async void Disconnect()
    {
        await Web3Unity.Instance.Disconnect();
    }
    
    public Task OnWeb3Initialized(Web3 web3)
    {
        connectButton.gameObject.SetActive(false);
        
        disconnectButton.gameObject.SetActive(true);

        addressText.text = web3.Signer.PublicAddress;

        return Task.CompletedTask;
    }

    public Task OnLogout()
    {
        connectButton.gameObject.SetActive(true);
        
        disconnectButton.gameObject.SetActive(false);
        
        addressText.text = string.Empty;

        return Task.CompletedTask;
    }

    public Web3Builder ConfigureServices(Web3Builder web3Builder)
    {
        return web3Builder.Configure(services =>
        {
            services.AddSingleton<ILogoutHandler>(this);
        });
    }
}
