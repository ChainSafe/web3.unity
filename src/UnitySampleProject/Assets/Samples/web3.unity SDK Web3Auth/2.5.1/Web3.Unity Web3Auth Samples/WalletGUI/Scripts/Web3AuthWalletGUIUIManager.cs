using ChainSafe.Gaming.UnityPackage;
using ChainSafe.GamingSdk.Web3Auth;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Web3AuthWalletGUIUIManager : MonoBehaviour
{
    #region Fields

    [SerializeField] private GameObject openWalletGUIContainer;
    [SerializeField] private GameObject walletGUIContainer;
    [SerializeField] private GameObject privateKeyContainer;
    [SerializeField] private TextMeshProUGUI walletAddressText;
    [SerializeField] private TextMeshProUGUI privateKeyText;
    [SerializeField] private Button openWalletButton;
    [SerializeField] private Button closeWalletButton;
    [SerializeField] private Button copyWalletAddressButton;
    [SerializeField] private Button openPrivateKeyMenuButton;
    [SerializeField] private Button closePrivateKeyMenuButton;
    [SerializeField] private Button copyPrivateKeyButton;
    [SerializeField] private Button holdToRevealPrivateKeyButton;
    [SerializeField] public GameObject walletLogoDisplay;
    [SerializeField] private Image circleLoadingImage;
    private bool isHeldDown;
    private float holdTime;
    private float holdDuration = 2f;

    #endregion

    #region Methods
    
    private void Awake()
    {
        openWalletButton.onClick.AddListener(ToggleWallet);
        closeWalletButton.onClick.AddListener(ToggleWallet);
        copyWalletAddressButton.onClick.AddListener(CopyWalletAddressButton);
        openPrivateKeyMenuButton.onClick.AddListener(TogglePrivateKeyMenuButton);
        closePrivateKeyMenuButton.onClick.AddListener(TogglePrivateKeyMenuButton);
        copyPrivateKeyButton.onClick.AddListener(CopyPrivateKeyButton);
        copyPrivateKeyButton.onClick.AddListener(HoldToRevealPrivateKeyButton);
        SetPrivateKey();
    }

    public void ToggleWallet()
    {
        walletGUIContainer.SetActive(!walletGUIContainer.activeSelf);
        openWalletGUIContainer.SetActive(!openWalletGUIContainer.activeSelf);
    }
    
    private void WalletToggleKeyInputCheck()
    {
        // Check for shift + tab press to allow opening and closing of wallet GUI
        if ((Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)) && Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleWallet();
        }
    }
    
    private void CopyWalletAddressButton()
    {
        Web3AuthWalletGUIClipboardManager.CopyText(walletAddressText.text);
    }
    
    private void HoldToRevealPrivateKeyButton()
    {
        holdToRevealPrivateKeyButton.gameObject.SetActive(false);
        copyPrivateKeyButton.gameObject.SetActive(true);
        privateKeyText.gameObject.SetActive(true);
    }
    
    private void TogglePrivateKeyMenuButton()
    {
        if (privateKeyContainer.activeSelf)
        {
            holdToRevealPrivateKeyButton.gameObject.SetActive(true);
            copyPrivateKeyButton.gameObject.SetActive(false);
            privateKeyText.gameObject.SetActive(false);
        }
        privateKeyContainer.SetActive(!privateKeyContainer.activeSelf);
    }
    
    private void SetPrivateKey()
    {
        var w3a = (Web3AuthWallet)Web3Accessor.Web3.ServiceProvider.GetService(typeof(Web3AuthWallet));
        privateKeyText.text = w3a.Key;
    }
    
    private void CopyPrivateKeyButton()
    {
        Web3AuthWalletGUIClipboardManager.CopyText(privateKeyText.text);
    }
    
    private void Update()
    {
        WalletToggleKeyInputCheck();
        // Check if the mouse button is held down
        if (Input.GetMouseButton(0))
        {
            if (!isHeldDown)
            {
                isHeldDown = true;
                holdTime = 0f;
            }
            else
            {
                holdTime += Time.deltaTime;
                // Calculate the fill amount based on the hold time
                float fillAmount = Mathf.Clamp01(holdTime / holdDuration);
                circleLoadingImage.fillAmount = fillAmount;

                // Complete the action if the circle is full
                if (fillAmount >= 1f)
                {
                    HoldToRevealPrivateKeyButton();
                }
            }
        }
        else
        {
            if (isHeldDown)
            {
                isHeldDown = false;
                circleLoadingImage.fillAmount = 0f; // Reset the circle fill
            }
        }
    }
    
    #endregion
}
