using System;
using System.Collections;
using System.Collections.Generic;
using MetaMask;
using MetaMask.Unity;
using UnityEngine;

public class MetaMaskController : MonoBehaviour
{
    private MetaMaskWallet _wallet;
    
    void Start()
    {
        _wallet = MetaMaskUnity.Instance.Wallet;
        _wallet.WalletConnectedHandler += OnWalletConnected;
        
        Connect();
    }

    private void OnWalletConnected(object sender, EventArgs e)
    {
        Debug.Log("Wallet Connected");
    }

    public void Connect()
    {
        _wallet.Connect();
    }
    
    public void Disconnect()
    {
        _wallet.Disconnect();
    }
}
