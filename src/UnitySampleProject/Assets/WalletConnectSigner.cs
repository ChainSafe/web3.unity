using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;
using WalletConnectSharp.Common.Utils;
using WalletConnectSharp.Network.Models;
using WalletConnectSharp.Sign.Models;

public class WalletConnectSigner : MonoBehaviour
{
    [SerializeField] private WalletConnectUnity _walletConnect;
    
    public async void SendSignRequest()
    {
        var (session, address, chainId) = GetCurrentAddress();
        if (string.IsNullOrWhiteSpace(address))
            return;
        
        var request = new EthSendTransaction(new Transaction()
        {
            From = address,
            To = address,
            Value = "0"
        });

        var result =
            await _walletConnect.SignClient.Request<EthSendTransaction, string>(session.Topic, request, chainId);
        
        MainThreadDispatcher.Instance.Invoke(delegate
        {
            Debug.LogError("Got result from request: " + result);
        });
    }
    
    public (SessionStruct, string, string) GetCurrentAddress()
    {
        var currentSession = _walletConnect.SignClient.Session.Get(_walletConnect.SignClient.Session.Keys[0]);

        var defaultChain = currentSession.Namespaces.Keys.FirstOrDefault();
            
        if (string.IsNullOrWhiteSpace(defaultChain))
            return (default, null, null);

        var defaultNamespace = currentSession.Namespaces[defaultChain];

        if (defaultNamespace.Accounts.Length == 0)
            return (default, null, null);
            
        var fullAddress = defaultNamespace.Accounts[0];
        var addressParts = fullAddress.Split(":");
            
        var address = addressParts[2];
        var chainId = string.Join(':', addressParts.Take(2));

        return (currentSession, address, chainId);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            SendSignRequest();
        }
    }
}