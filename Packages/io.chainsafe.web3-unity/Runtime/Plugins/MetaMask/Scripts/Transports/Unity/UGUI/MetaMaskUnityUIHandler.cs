using System;
using System.Collections;
using MetaMask.Models;

using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace MetaMask.Transports.Unity.UI
{

    public class MetaMaskUnityUIHandler : MonoBehaviour, IMetaMaskUnityTransportListener
    {
        #region Events

        /// <summary>Occurs when the application's open state changes.</summary>
        public event EventHandler OpenStateChanged;

        #endregion

        #region Fields

        /// <summary>The CanvasGroup component attached to the root GameObject.</summary>
        [SerializeField]
        protected CanvasGroup canvasGroup;

        [SerializeField]
        protected GameObject qrCodePanel;
        /// <summary>Gets or sets a value indicating whether the dropdown is open.</summary>
        /// <value>true if the dropdown is open; otherwise, false.</value>
        [SerializeField]
        protected bool isOpen = false;

        /// <summary>The QR image.</summary>
        [SerializeField]
        protected MetaMaskUnityUIQRImage qrImage;

        [SerializeField]
        protected MetaMaskOTPPanel otpPanel;

        [SerializeField]
        protected GameObject background;

        [SerializeField]
        protected float fadeDuration = 0.5f;

        #endregion

        #region Properties

        /// <summary>Gets a value indicating whether the UI is open.</summary>
        /// <returns>true if the UI is open; otherwise, false.</returns>
        public virtual bool IsOpen => this.isOpen;

        #endregion

        #region Protected Methods
        /// <summary>Returns whether a UI is opened.</summary>
        protected virtual void OnQRCodeOpenStateChanged()
        {
            this.qrCodePanel.SetActive(this.isOpen);
            this.canvasGroup.interactable = this.isOpen;
            this.canvasGroup.blocksRaycasts = this.isOpen;
            this.canvasGroup.alpha = this.isOpen ? 1f : 0f;

            StartCoroutine(FadeBackground(this.canvasGroup.alpha));

            OpenStateChanged?.Invoke(this, null);
        }

        #endregion

        #region Public Methods

        /// <summary>Opens the clipboard.</summary>
        public virtual void OpenQRCode()
        {
            SetOpenQRCode(true);
        }

        /// <summary>Closes the window.</summary>
        public virtual void CloseQRCode()
        {
            SetOpenQRCode(false);
        }

        /// <summary>Toggles the open state of the menu.</summary>
        public virtual void ToggleOpenQRCode()
        {
            SetOpenQRCode(!this.isOpen);
        }

        /// <summary>Sets the open state of the menu.</summary>
        /// <param name="open">Whether the menu is open.</param>
        public void SetOpenQRCode(bool open)
        {
            if (this.isOpen == open)
            {
                return;
            }
            this.isOpen = open;
            OnQRCodeOpenStateChanged();
        }

        /// <summary>Called when the MetaMask client wants to connect to the application.</summary>
        /// <param name="url">The URL to connect to.</param>
        public void OnMetaMaskConnectRequest(string universalLink, string deepLink)
        {
            this.qrImage.ShowQR(universalLink);
        }

        /// <summary>Handles a MetaMask request.</summary>
        /// <param name="id">The request ID.</param>
        /// <param name="request">The request.</param>
        public void OnMetaMaskRequest(string id, MetaMaskEthereumRequest request)
        {
        }

        /// <summary>Called when the MetaMask client encounters an error.</summary>
        /// <param name="error">The error that occurred.</param>
        public void OnMetaMaskFailure(Exception error)
        {
        }

        /// <summary>Called when the MetaMask login was successful.</summary>
        public void OnMetaMaskSuccess()
        {
            CloseQRCode();
            if (otpPanel != null)
            {
                otpPanel.gameObject.SetActive(false);
            }
        }

        public void OnMetaMaskOTP(int otp)
        {
            if (otpPanel != null)
            {
                otpPanel.gameObject.SetActive(true);
                otpPanel.ShowOTP(otp);
                qrCodePanel.SetActive(false);
                
                StartCoroutine(FadeBackground(1f));
            }
            else
            {
                throw new Exception("No OTP UI Panel found");
            }
        }

        public void OnMetaMaskDisconnected()
        {
            CloseQRCode();
            if (otpPanel != null)
            {
                otpPanel.gameObject.SetActive(false);
                StartCoroutine(FadeBackground(0f));
            }
        }

        #endregion

        #region Private Methods

        private IEnumerator FadeBackground(float targetAlpha)
        {
            if (targetAlpha == 1f)
            {
                targetAlpha = 0.75f;
            }

            var image = background.GetComponent<Image>();
            var canvasGroup = background.GetComponent<CanvasGroup>();

            if (image)
                return FadeBackgroundImage(targetAlpha, image);
            if (canvasGroup)
                return FadeBackgroundCanvasGroup(targetAlpha, canvasGroup);
            
            throw new Exception("Supplied background gameObject " + background.name +
                                " has no Image or CanvasGroup component");
        }

        private IEnumerator FadeBackgroundCanvasGroup(float targetAlpha, CanvasGroup backgroundCanvasGroup)
        {
            float startAlpha = backgroundCanvasGroup.alpha;
            float startTime = Time.time;

            while (backgroundCanvasGroup.alpha != targetAlpha)
            {
                float duration = (Time.time - startTime) / fadeDuration;
                float a = Mathf.Lerp(startAlpha, targetAlpha, duration);

                if (duration >= fadeDuration)
                {
                    a = targetAlpha;
                }

                backgroundCanvasGroup.alpha = a;

                yield return new WaitForEndOfFrame();
            }
        }

        private IEnumerator FadeBackgroundImage(float targetAlpha, Image backgroundImage)
        {
            float startAlpha = backgroundImage.color.a;
            float startTime = Time.time;

            while (backgroundImage.color.a != targetAlpha)
            {
                float duration = (Time.time - startTime) / fadeDuration;
                float a = Mathf.Lerp(startAlpha, targetAlpha, duration);

                if (duration >= fadeDuration)
                {
                    a = targetAlpha;
                }

                backgroundImage.color = new Color(backgroundImage.color.r, backgroundImage.color.g,
                    backgroundImage.color.b, a);

                yield return new WaitForEndOfFrame();
            }
        }

        #endregion

    }

}