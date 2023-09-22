using System;
using ChainSafe.Gaming.UnityPackage;
using ChainSafe.Gaming.Wallets;
using ChainSafe.Gaming.Wallets.WalletConnect;
using ChainSafe.Gaming.Web3;
using QRCoder;
using QRCoder.Unity;
using Scenes;
using UnityEngine;
using UnityEngine.UI;
using WalletConnectSharp.Sign.Models;
using WalletConnectSharp.Sign.Models.Engine;
using Web3Unity.Scripts;

public class WalletConnectModal : MonoBehaviour
{
    [SerializeField] private Image _qrCodeImage;
    [SerializeField] private Button _copyToClipboardButton;
    [SerializeField] private Button _backButton;
    
    [SerializeField] private Transform _container;

    private void Start()
    {
        _backButton.onClick.AddListener(Disable);
        
        WebPageWallet.OnConnected += WalletConnected;
        
        WebPageWallet.OnSessionApproved += SessionApproved;
    }

    private void WalletConnected(ConnectedData data)
    {
        MainThreadDispatcher.Instance.Invoke(delegate
        {
            // enable display
            _container.gameObject.SetActive(true);

            string uri = data.Uri;
                
            GenerateQrCode(uri);
                
            SetClipboard(uri);
        });
    }
    
    private void SessionApproved(SessionStruct session)
    {
        MainThreadDispatcher.Instance.Invoke(delegate
        {
            // disable display
            Disable();
            
            Debug.Log($"{session.Topic} Approved");
        });
    }
    
    private void SetClipboard(string uri)
    {
        _copyToClipboardButton.onClick.RemoveAllListeners();
        
        _copyToClipboardButton.onClick.AddListener(delegate
        {
            GUIUtility.systemCopyBuffer = uri;
        });
    }
    
    private void GenerateQrCode(string uri)
    {
        Debug.Log("Connecting to: " + uri);
        QRCodeGenerator generator = new QRCodeGenerator();
        QRCodeData data = generator.CreateQrCode(uri, QRCodeGenerator.ECCLevel.Q);
        UnityQRCode qrCode = new UnityQRCode(data);

        // pixelsPerModule:10 gives us a 690x690 pixel image.
        Texture2D qrCodeAsTexture2D = qrCode.GetGraphic(pixelsPerModule:10);

        // Change the filtering mode to point (i.e. nearest) rather than the default of linear - we want sharp edges on
        // the blocks, not blurry interpolated edges!
        qrCodeAsTexture2D.filterMode = FilterMode.Point;

        // Convert the texture into a sprite and assign it to our QR code image
        var qrCodeSprite = Sprite.Create(qrCodeAsTexture2D, new Rect(0, 0, qrCodeAsTexture2D.width, qrCodeAsTexture2D.height),
            new Vector2(0.5f, 0.5f), 100f);
        _qrCodeImage.sprite = qrCodeSprite;
    }

    private void Disable()
    {
        _container.gameObject.SetActive(false);
    }
    
    private void OnDisable()
    {
        WebPageWallet.OnConnected -= WalletConnected;
        
        WebPageWallet.OnSessionApproved -= SessionApproved;
    }
}
