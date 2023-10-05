using ChainSafe.Gaming.WalletConnect;
using UnityEngine;
using UnityEngine.UI;
using WalletConnectSharp.Sign.Models;
using WalletConnectSharp.Sign.Models.Engine;
using ZXing;
using ZXing.QrCode;

public class WalletConnectModal : MonoBehaviour
{
    [SerializeField] private Image _qrCodeImage;
    [SerializeField] private Button _copyToClipboardButton;
    [SerializeField] private Button _backButton;

    [SerializeField] private Transform _container;

    private void Start()
    {
        _backButton.onClick.AddListener(Disable);

        WalletConnectSigner.OnConnected += WalletConnected;

        WalletConnectSigner.OnSessionApproved += SessionApproved;
    }

    private void OnDisable()
    {
        WalletConnectSigner.OnConnected -= WalletConnected;

        WalletConnectSigner.OnSessionApproved -= SessionApproved;
    }
    
    private void WalletConnected(ConnectedData data)
    {
        if (Application.isMobilePlatform)
        {
#if UNITY_ANDROID
            
            // open wallet
            Application.OpenURL(data.Uri);
            
#elif UNITY_IOS
            //select wallet dropdown
#endif
        }
            
        // enable display
        _container.gameObject.SetActive(true);

        string uri = data.Uri;

        GenerateQrCode(uri);

        SetClipboard(uri);
    }

    private void SessionApproved(SessionStruct session)
    {
        Debug.Log($"{session.Topic} Approved");
    }

    private void SetClipboard(string uri)
    {
        _copyToClipboardButton.onClick.RemoveAllListeners();

        _copyToClipboardButton.onClick.AddListener(delegate { GUIUtility.systemCopyBuffer = uri; });
    }

    private static Color32[] Encode(string textForEncoding, int width, int height)
    {
        var writer = new BarcodeWriter
        {
            Format = BarcodeFormat.QR_CODE, Options = new QrCodeEncodingOptions { Height = height, Width = width }
        };
        return writer.Write(textForEncoding);
    }

    private void GenerateQrCode(string text)
    {
        var encoded = new Texture2D(256, 256);
        var color32 = Encode(text, encoded.width, encoded.height);
        encoded.SetPixels32(color32);
        encoded.Apply();
        
        // Convert the texture into a sprite and assign it to our QR code image
        var qrCodeSprite = Sprite.Create(encoded, new Rect(0, 0, encoded.width, encoded.height),
            new Vector2(0.5f, 0.5f), 100f);
        _qrCodeImage.sprite = qrCodeSprite;
    }

    private void Disable()
    {
        _container.gameObject.SetActive(false);
    }
}