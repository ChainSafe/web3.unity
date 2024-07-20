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

    private void Awake()
    {
        _connectionHandler = GetComponent<ConnectionHandler>();
    }

    private async void Start()
    {
        _connectionHandler.Initialize();
        
        if (connectOnInitialize)
        {
            var web3 = await _connectionHandler.Restore();

            if (web3 != null)
            {
                Web3Accessor.Set(web3);
                
                return;
            }
        }
        
        _connectModal = Instantiate(connectModalPrefab);
        
        await _connectModal.Initialize(_connectionHandler);
        
        connectButton.onClick.AddListener(_connectModal.Show);
    }
}
