using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ChainSafe.Gaming
{
    public class ConnectModal : MonoBehaviour
    {
        [SerializeField] private ErrorOverlay errorOverlay;
        [SerializeField] private LoadingOverlay loadingOverlay;
        [SerializeField] private Button closeButton;
        // Closes modal when background is clicked
        [SerializeField] private Button closeFromBackgroundButton;

        private void Start()
        {
            closeButton.onClick.AddListener(Close);
            closeFromBackgroundButton.onClick.AddListener(Close);
        }

        private void DisplayError(string message)
        {
            errorOverlay.DisplayError(message);
        }
        
        private void ShowLoading()
        {
            loadingOverlay.gameObject.SetActive(true);
        }
        
        private void HideLoading()
        {
            loadingOverlay.gameObject.SetActive(false);
        }
        
        private void Close()
        {
            gameObject.SetActive(false);
        }
    }
}
