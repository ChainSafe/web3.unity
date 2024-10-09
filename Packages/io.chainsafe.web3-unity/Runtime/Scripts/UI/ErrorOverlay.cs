using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ChainSafe.Gaming.UnityPackage.UI
{
    /// <summary>
    /// Error overlay used for displaying error messages.
    /// </summary>
    public class ErrorOverlay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI errorText;
        [SerializeField] private Button closeButton;

        private void Start()
        {
            closeButton.onClick.AddListener(Close);
        }

        /// <summary>
        /// Display error messages.
        /// </summary>
        /// <param name="message">Error message to display.</param>
        public void DisplayError(string message)
        {
            gameObject.SetActive(true);

            errorText.SetText(message);
        }

        /// <summary>
        /// Close error overlay.
        /// </summary>
        private void Close()
        {
            gameObject.SetActive(false);
        }
    }
}
