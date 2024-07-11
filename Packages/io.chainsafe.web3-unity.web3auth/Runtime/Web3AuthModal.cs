using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Web3AuthModal : MonoBehaviour
{
    [SerializeField] private Button closeButton;
    // Closes modal when background is clicked
    [SerializeField] private Button closeFromBackgroundButton;
    
    private void Start()
    {
        closeButton.onClick.AddListener(Close);
        closeFromBackgroundButton.onClick.AddListener(Close);
    }
    
    private void Close()
    {
        gameObject.SetActive(false);
    }
}
