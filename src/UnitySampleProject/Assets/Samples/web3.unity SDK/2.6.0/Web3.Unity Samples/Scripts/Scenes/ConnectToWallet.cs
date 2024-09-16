using ChainSafe.Gaming.UnityPackage;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Used to easily connect to wallet.
/// </summary>
public class ConnectToWallet : MonoBehaviour
{
    [SerializeField] private bool connectOnInitialize = true;
    [SerializeField] private Button connectButton;

    private async void Start()
    {
        await Web3Unity.Instance.Initialize(connectOnInitialize);
        
        if (!Web3Unity.Connected)
        {
            connectButton.onClick.AddListener(Web3Unity.ConnectModal.Open);
            
            connectButton.interactable = true;
        }
    }
}
