using System;
using ChainSafe.Gaming.UnityPackage;
using ChainSafe.Gaming.UnityPackage.Connection;
using ChainSafe.Gaming.UnityPackage.UI;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ConnectionHandler))]
public class ConnectToWallet : MonoBehaviour
{
    [SerializeField] private bool connectOnInitialize;
    [SerializeField] private ConnectModal connectModalPrefab;
    
    [SerializeField] private Button connectButton;

    private ConnectionHandler _connectionHandler;
    private ConnectModal _connectModal;

    private bool _connected;
    
    private void Awake()
    {
        _connectionHandler = GetComponent<ConnectionHandler>();
    }

    private async void Start()
    {
        await _connectionHandler.Initialize();

        try
        {
            if (connectOnInitialize)
            {
                var web3 = await _connectionHandler.Restore();
                
                _connected = web3 != null;
            }
        }
        finally
        {
            if (!_connected)
            {
                _connectModal = Instantiate(connectModalPrefab);
        
                _connectModal.Initialize(_connectionHandler);
        
                connectButton.onClick.AddListener(_connectModal.Show);
        
                connectButton.interactable = true;
            }
        }
    }
}
