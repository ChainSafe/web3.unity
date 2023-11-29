using System;
using MetaMask.Unity;
using TMPro;
using UnityEngine;

namespace MetaMask.Transports.Unity.UI
{
    public class MetaMaskOTPPanel : MonoBehaviour
    {
        public TextMeshProUGUI codeText;

        public GameObject otpCodeDisplay;
        public GameObject simpleTextDisplay;

        public bool ShouldShowOTP => DateTime.Now - MetaMaskUnity.Instance.Wallet.LastActive >= TimeSpan.FromHours(1);

        public void OnDisconnect()
        {
            MetaMaskUnity.Instance.EndSession();
            
            gameObject.SetActive(false);
        }

        public void ShowOTP(int code)
        {
            codeText.text = code.ToString();

            if (!gameObject.activeSelf)
            {
                gameObject.SetActive(true);
            }

            otpCodeDisplay.SetActive(ShouldShowOTP);
            simpleTextDisplay.SetActive(!ShouldShowOTP);
        }
    }
}