using UnityEngine;
using ZXing;
using ZXing.QrCode;

namespace ChainSafe.Gaming.WalletConnect.Dialog
{
    public class QrCodeBuilder
    {
        private readonly string url;

        public QrCodeBuilder(string url)
        {
            this.url = url;
        }

        public Texture2D GenerateQrCode(QrCodeEncodingOptions encodingOptions)
        {
            return GenerateQrCode(url, encodingOptions);
        }

        public static Texture2D GenerateQrCode(string url, QrCodeEncodingOptions encodingOptions)
        {
            var writer = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,
                Options = encodingOptions
            };
            var textureRaw = writer.Write(url);
            var texture = new Texture2D(encodingOptions.Width, encodingOptions.Height);
            texture.SetPixels32(textureRaw);
            texture.filterMode = FilterMode.Trilinear;
            texture.Apply();
            return texture;
        }
    }
}