using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;
using WalletConnectSharp.Common.Utils;
using WalletConnectSharp.Network.Models;
using WalletConnectSharp.Sign.Models;
using WalletConnectSharp.Sign.Models.Engine;

public class WalletConnectSigner : MonoBehaviour
{
    [SerializeField] private WalletConnectUnity _walletConnect;

    private ConnectedData _cachedConnectedData;
    
    private void Start()
    {
        _walletConnect.OnConnected += data =>
        {
            _cachedConnectedData = data;
        };
    }

    public async void SendTransaction(string to, string value)
    {
        var (session, address, chainId) = GetCurrentAddress();
        if (string.IsNullOrWhiteSpace(address))
            return;
        
        var request = new EthSendTransaction(new Transaction()
        {
            From = address,
            To = to,
            Value = value
        });
        
        var result =
            await _walletConnect.SignClient.Request<EthSendTransaction, string>(session.Topic, request, chainId);
        
        MainThreadDispatcher.Instance.Invoke(delegate
        {
            Debug.Log($"eth_sendTransaction result: {result}");
        });
    }
    
    public async void SignMessage(string message)
    {
        var (session, address, chainId) = GetCurrentAddress();
        if (string.IsNullOrWhiteSpace(address))
            return;
        
        var request = new EthSignMessage(message, address);
        
        var result =
            await _walletConnect.SignClient.Request<EthSignMessage, string>(session.Topic, request, chainId);
        
        MainThreadDispatcher.Instance.Invoke(delegate
        {
            Debug.Log($"personal_sign result: {result}");
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
            SignMessage("message to sign");
        }
    }
}