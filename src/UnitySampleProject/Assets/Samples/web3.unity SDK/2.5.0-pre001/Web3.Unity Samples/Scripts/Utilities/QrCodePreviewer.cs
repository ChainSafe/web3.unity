using System;
using ChainSafe.Gaming.UnityPackage;
using ChainSafe.Gaming.Wallets;
using ChainSafe.Gaming.Wallets.WalletConnect;
using ChainSafe.Gaming.Web3;
using Scenes;
using UnityEngine;
using UnityEngine.UI;
using Web3Unity.Scripts;

public class QrCodePreviewer : MonoBehaviour
{
    [SerializeField] private Image _qrCodeImage;

    private void Start()
    {
        // Initialize when web3 is initialized
        Web3Accessor.Instance.OnWeb3Initialized += Initialize;
    }

    public void Initialize()
    {
        WebPageWallet wallet = Web3Accessor.Web3.Signer as WebPageWallet;

        if (wallet == null)
        {
            Debug.LogError("Wallet not Initialized");
            
            return;
        }
        
        wallet.WalletConnectUnity.OnConnected += data =>
        {
            MainThreadDispatcher.Instance.Invoke(delegate
            {
                Debug.LogError(data.Uri);
            });
        };
        
        wallet.WalletConnectUnity.OnSessionApproved += session =>
        {
            MainThreadDispatcher.Instance.Invoke(delegate
            {
                Debug.LogError(session.Peer.PublicKey);
            
                Debug.LogError(session.Self.PublicKey);
            });
        };
    }
}
