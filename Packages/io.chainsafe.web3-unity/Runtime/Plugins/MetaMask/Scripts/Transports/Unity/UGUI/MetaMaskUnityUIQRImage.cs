using System;
using System.Collections;
using MetaMask.Models;
using UnityEngine;
using UnityEngine.UI;
using ZXing;
using ZXing.QrCode;

namespace MetaMask.Transports.Unity.UI
{

    public class MetaMaskUnityUIQRImage : MonoBehaviour, IMetaMaskUnityTransportListener
    {
        #region Fields
        /// <summary>The raw image to display.</summary>
        [SerializeField]
        protected RawImage rawImage;
        /// <summary>Should we show the QR code if the deeplink is enabled.</summary>
        [SerializeField]
        private bool showQrCodeOnDeeplink = true;
        /// <summary>The texture to use for deep links.</summary>
        [SerializeField]
        private Sprite[] deepLinkTexture;
        /// <summary>Gets or sets the index of the current sprite.</summary>
        private int _currentSpriteIndex = 0;
        /// <summary>Gets a value indicating whether the application is currently animating.</summary>
        /// <returns>true if the application is currently animating; otherwise, false.</returns>
        private bool _isAnimating = false;
        /// <summary>Gets or sets a value indicating whether the QRSwitch is on.</summary>
        private bool _QRSwitch = false;
        /// <summary>Gets the last URL that was copied to the script.</summary>
        /// <returns>The last URL that was copied to the script.</returns>
        private string _lastUrl = string.Empty;
        #endregion

        #region Unity Methods

          /// <summary>Starts the deeplink Animation if it should be on.</summary>
        private void OnEnable()
        {
            if (this._isAnimating)
            {
                StartCoroutine(Animate(this.rawImage));
            }
            
        }
        /// <summary>Stops all currently active Coroutines.</summary>
        private void OnDisable()
        {
            StopAllCoroutines();
        }

        #endregion
        
        #region Private Methods
        /// <summary>Resets the image to its original state.</summary>
        private void Reset()
        {
            this.rawImage = GetComponent<RawImage>();
        }

        /// <summary>Encodes the given text into a QR code.</summary>
        /// <param name="textForEncoding">The text to encode.</param>
        /// <param name="width">The width of the QR code.</param>
        /// <param name="height">The height of the QR code.</param>
        /// <returns>The QR code as a 2D array of colors.</returns>
        private static Color32[] EncodeToQR(string textForEncoding, int width, int height)
        {
            var writer = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new QrCodeEncodingOptions
                {
                    Height = height,
                    Width = width
                }
            };
            return writer.Write(textForEncoding);
        }

        /// <summary>Generates a QR code texture.</summary>
        /// <param name="text">The text to encode.</param>
        /// <returns>The QR code texture.</returns>
        private static Texture2D GenerateQRTexture(string text)
        {
            Texture2D encoded = new Texture2D(256, 256);
            var color32 = EncodeToQR(text, encoded.width, encoded.height);
            encoded.SetPixels32(color32);
            encoded.Apply();
            return encoded;
        }

        #endregion

        #region Public Methods
        /// <summary>Shows a QR code for the specified URL.</summary>       
        /// <param name="url">The URL to show in the QR code.</param>       
        public void ShowQR(string url)
        {
            // make sure to add t=q to end of url
            var tq = "&t=q";
            if (!url.EndsWith(tq))
                url += tq;
            
            StopAllCoroutines();
            this._lastUrl = url;
            if (MetaMaskUnityUITransport.DefaultInstance.IsDeeplinkAvailable() && !showQrCodeOnDeeplink && Application.isMobilePlatform)
            {
                if (gameObject.activeInHierarchy)
                {
                    this._isAnimating = true;
                    StartCoroutine(Animate(this.rawImage));
                }
                else
                {
                    this._isAnimating = true;
                }

                return;
            }

            this.rawImage.texture = GenerateQRTexture(url);
        }

        /// <summary>Called when the MetaMask client wants to connect to the application.</summary>
        /// <param name="url">The URL to connect to.</param>
        public void OnMetaMaskConnectRequest(string universalLink, string deepLink)
        {
            ShowQR(universalLink);
        }

        /// <summary>Called when a MetaMask request is received.</summary>
        /// <param name="id">The request ID.</param>
        /// <param name="request">The request.</param>
        public void OnMetaMaskRequest(string id, MetaMaskEthereumRequest request)
        {
        }

        /// <summary>Called when MetaMask fails to connect.</summary>
        /// <param name="error">The error that occurred.</param>
        public void OnMetaMaskFailure(Exception error)
        {
        }

        /// <summary>Called when the MetaMask login was successful.</summary>
        public void OnMetaMaskSuccess()
        {
        }

        public void OnMetaMaskOTP(int otp)
        {
        }

        public void OnMetaMaskDisconnected()
        {
        }

        /// <summary>Switches the mode of the application.</summary>
        public void SwitchMode()
        {
            if(!Application.isMobilePlatform)
            {
                return;
            }
            
            if (this._lastUrl != "")
            {
                
                if (this.showQrCodeOnDeeplink)
                {
                    this.showQrCodeOnDeeplink = false;
                    ShowQR(this._lastUrl);
                }
                else
                {
                    this.showQrCodeOnDeeplink = true;
                    ShowQR(this._lastUrl);
                }
            }
            
        }

        #endregion

        #region Coroutine
        /// <summary>Animates the image.</summary>
        /// <param name="image">The image to animate.</param>
        /// <param name="animationSpeed">The speed of the animation.</param>
        /// <returns>An enumerator that can be used to animate the image.</returns>
        IEnumerator Animate(RawImage image,float animationSpeed = 0.1f) {
            while (true) {
                image.texture = this.deepLinkTexture[_currentSpriteIndex].texture;
                _currentSpriteIndex = (_currentSpriteIndex + 1) % this.deepLinkTexture.Length;
                yield return new WaitForSeconds(animationSpeed);
            }
        }

        #endregion

    }

}