using System;
using ChainSafe.Gaming.UnityPackage;
using ChainSafe.Gaming.UnityPackage.Connection;
using ChainSafe.Gaming.UnityPackage.UI;
using UnityEngine;
using UnityEngine.UI;

public class ConnectToWallet : MonoBehaviour
{
    [SerializeField] private Button connectButton;

    private async void Start()
    {
        await ChainSafeManager.Instance.Initialize();

        if (!ChainSafeManager.Instance.Connected)
        {
            connectButton.onClick.AddListener(ChainSafeManager.Instance.ShowModal);
            
            connectButton.interactable = true;
        }
    }
}
