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

    public void Start()
    {
        WebPageWallet.OnConnected += data =>
        {
            MainThreadDispatcher.Instance.Invoke(delegate
            {
                Debug.LogError(data.Uri);
            });
        };
        
        WebPageWallet.OnSessionApproved += session =>
        {
            MainThreadDispatcher.Instance.Invoke(delegate
            {
                Debug.LogError(session.Peer.PublicKey);
            
                Debug.LogError(session.Self.PublicKey);
            });
        };
    }
}
