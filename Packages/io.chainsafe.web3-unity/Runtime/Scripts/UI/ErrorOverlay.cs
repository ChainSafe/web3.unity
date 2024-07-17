using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ChainSafe.Gaming
{
    public class ErrorOverlay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI errorText;
        [SerializeField] private Button closeButton;

        private void Start()
        {
            closeButton.onClick.AddListener(Close);
        }

        public void DisplayError(string message)
        {
            gameObject.SetActive(true);
            
            errorText.SetText(message);
        }
        
        public void Close()
        {
            gameObject.SetActive(false);
        }
    }
}
