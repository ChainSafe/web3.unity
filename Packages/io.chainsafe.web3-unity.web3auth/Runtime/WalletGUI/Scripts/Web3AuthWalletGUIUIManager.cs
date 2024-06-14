using ChainSafe.Gaming.UnityPackage;
using ChainSafe.GamingSdk.Web3Auth;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Application = UnityEngine.Device.Application;

public class Web3AuthWalletGUIUIManager : MonoBehaviour
{
    #region Fields
    
    [SerializeField] private Web3AuthWalletGUITxManager txManager;
    [SerializeField] private GameObject openWalletGUIContainer;
    [SerializeField] private GameObject walletGUIContainer;
    [SerializeField] private GameObject privateKeyContainer;
    [SerializeField] public GameObject walletLogoDisplay;
    [SerializeField] private TextMeshProUGUI walletAddressText;
    [SerializeField] private TextMeshProUGUI privateKeyText;
    [SerializeField] private Button openWalletButton;
    [SerializeField] private Button closeWalletButton;
    [SerializeField] private Button copyWalletAddressButton;
    [SerializeField] private Button openPrivateKeyMenuButton;
    [SerializeField] private Button closePrivateKeyMenuButton;
    [SerializeField] private Button copyPrivateKeyButton;
    [SerializeField] private Button holdToRevealPrivateKeyButton;
    [SerializeField] private Image circleLoadingImage;
    [SerializeField] private Toggle autoTxToggle;
    private bool isHeldDown;
    private float holdTime;
    private float holdDuration = 2f;
    public bool displayWalletIcon { get; set; }

    #endregion

    #region Methods
    
    /// <summary>
    /// Initializes objects.
    /// </summary>
    private void Awake()
    {
        openWalletButton.onClick.AddListener(ToggleWallet);
        closeWalletButton.onClick.AddListener(ToggleWallet);
        copyWalletAddressButton.onClick.AddListener(CopyWalletAddressButton);
        openPrivateKeyMenuButton.onClick.AddListener(TogglePrivateKeyMenuButton);
        closePrivateKeyMenuButton.onClick.AddListener(TogglePrivateKeyMenuButton);
        copyPrivateKeyButton.onClick.AddListener(CopyPrivateKeyButton);
        copyPrivateKeyButton.onClick.AddListener(HoldToRevealPrivateKeyButton);
        walletAddressText.text = Web3Accessor.Web3.Signer.PublicAddress;
        autoTxToggle.onValueChanged.AddListener(delegate { ToggleAutoConfirmTransactions(); });
        SetPrivateKey();
    }
    
    /// <summary>
    /// Toggles the wallet display.
    /// </summary>
    public void ToggleWallet()
    {
        walletGUIContainer.SetActive(!walletGUIContainer.activeSelf);
        if (!displayWalletIcon) return;
        openWalletGUIContainer.SetActive(!openWalletGUIContainer.activeSelf);
    }
    
    /// <summary>
    /// Called when tx toggle is changed
    /// </summary>
    private void ToggleAutoConfirmTransactions()
    {
        txManager.autoConfirmTransactions = autoTxToggle.isOn;
    }
    
    /// <summary>
    /// Keyboard input to toggle wallet display.
    /// </summary>
    private void WalletToggleKeyInputCheck()
    {
        // Check for shift + tab press to allow opening and closing of wallet GUI
        if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleWallet();
        }
    }
    
    /// <summary>
    /// Button function to copy the wallet address.
    /// </summary>
    private void CopyWalletAddressButton()
    {
        Web3AuthWalletGUIClipboardManager.CopyText(walletAddressText.text);
    }
    
    /// <summary>
    /// Reveals the private key when held down.
    /// </summary>
    private void HoldToRevealPrivateKeyButton()
    {
        holdToRevealPrivateKeyButton.gameObject.SetActive(false);
        copyPrivateKeyButton.gameObject.SetActive(true);
        privateKeyText.gameObject.SetActive(true);
    }
    
    /// <summary>
    /// Toggles the private key menu.
    /// </summary>
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
    
    /// <summary>
    /// Sets the private key text.
    /// </summary>
    private void SetPrivateKey()
    {
        var w3a = (Web3AuthWallet)Web3Accessor.Web3.ServiceProvider.GetService(typeof(Web3AuthWallet));
        privateKeyText.text = w3a.Key;
    }
    
    /// <summary>
    /// Button function to copy the wallet private key.
    /// </summary>
    private void CopyPrivateKeyButton()
    {
        Web3AuthWalletGUIClipboardManager.CopyText(privateKeyText.text);
    }
    
    /// <summary>
    /// Checks for wallet toggle input & tracks private key held timer.
    /// </summary>
    private void Update()
    {
        WalletToggleKeyInputCheck();
        // Check for mouse input
        bool isInputHeld = Input.GetMouseButton(0);
        // Check for touch input
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            isInputHeld = (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved);
        }
        if (isInputHeld)
        {
            if (!isHeldDown)
            {
                isHeldDown = true;
                holdTime = 0f;
            }
            else
            {
                holdTime += Time.deltaTime;
                float fillAmount = Mathf.Clamp01(holdTime / holdDuration);
                circleLoadingImage.fillAmount = fillAmount;
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
                circleLoadingImage.fillAmount = 0f;
            }
        }
    }
    
    #endregion
}
