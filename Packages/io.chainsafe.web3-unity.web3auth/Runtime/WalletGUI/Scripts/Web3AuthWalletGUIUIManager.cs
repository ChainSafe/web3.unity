using ChainSafe.Gaming.UnityPackage;
using ChainSafe.GamingSdk.Web3Auth;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Web3Auth wallet GUI manager to handle UI elements.
/// </summary>
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

    #endregion

    #region Properties

    public bool DisplayWalletIcon { get; set; }

    #endregion

    #region Methods
    
    /// <summary>
    /// Initializes objects.
    /// </summary>
    private void Awake()
    {
        InitializeButtons();
        walletAddressText.text = Web3Accessor.Web3.Signer.PublicAddress;
        autoTxToggle.onValueChanged.AddListener(delegate { ToggleAutoConfirmTransactions(); });
        SetPrivateKey();
    }
    
    /// <summary>
    /// Initializes button listeners
    /// </summary>
    private void InitializeButtons()
    {
        openWalletButton.onClick.AddListener(ToggleWallet);
        closeWalletButton.onClick.AddListener(ToggleWallet);
        copyWalletAddressButton.onClick.AddListener(CopyWalletAddress);
        openPrivateKeyMenuButton.onClick.AddListener(TogglePrivateKeyMenu);
        closePrivateKeyMenuButton.onClick.AddListener(TogglePrivateKeyMenu);
        copyPrivateKeyButton.onClick.AddListener(CopyPrivateKey);
        holdToRevealPrivateKeyButton.onClick.AddListener(RevealPrivateKey);
    }
    
    /// <summary>
    /// Toggles the wallet display.
    /// </summary>
    public void ToggleWallet()
    {
        walletGUIContainer.SetActive(!walletGUIContainer.activeSelf);
        if (DisplayWalletIcon)
        {
            openWalletGUIContainer.SetActive(!openWalletGUIContainer.activeSelf);
        }
    }

    /// <summary>
    /// Called when tx toggle is changed
    /// </summary>
    private void ToggleAutoConfirmTransactions()
    {
        txManager.AutoConfirmTransactions = autoTxToggle.isOn;
    }

    /// <summary>
    /// Checks for keyboard input to toggle wallet display.
    /// </summary>
    private void CheckWalletToggleKeyInput()
    {
        if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleWallet();
        }
    }

    /// <summary>
    /// Copies the wallet address to clipboard.
    /// </summary>
    private void CopyWalletAddress()
    {
        Web3AuthWalletGUIClipboardManager.CopyText(walletAddressText.text);
    }

    /// <summary>
    /// Reveals the private key when held down.
    /// </summary>
    private void RevealPrivateKey()
    {
        holdToRevealPrivateKeyButton.gameObject.SetActive(false);
        copyPrivateKeyButton.gameObject.SetActive(true);
        privateKeyText.gameObject.SetActive(true);
    }

    /// <summary>
    /// Toggles the private key menu.
    /// </summary>
    private void TogglePrivateKeyMenu()
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
        var web3AuthWallet = (Web3AuthWallet)Web3Accessor.Web3.ServiceProvider.GetService(typeof(Web3AuthWallet));
        privateKeyText.text = web3AuthWallet.Key;
    }

    /// <summary>
    /// Copies the wallet private key to clipboard.
    /// </summary>
    private void CopyPrivateKey()
    {
        Web3AuthWalletGUIClipboardManager.CopyText(privateKeyText.text);
    }

    /// <summary>
    /// Checks for mouse or touch input on private key menu
    /// </summary>
    private void CheckHoldPrivateKeyButtonInput()
    {
        bool isInputHeld = Input.GetMouseButton(0);
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            isInputHeld = touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved;
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
                circleLoadingImage.fillAmount = Mathf.Clamp01(holdTime / holdDuration);
                if (circleLoadingImage.fillAmount >= 1f)
                {
                    RevealPrivateKey();
                }
            }
        }
        else if (isHeldDown)
        {
            isHeldDown = false;
            circleLoadingImage.fillAmount = 0f;
        }
    }

    /// <summary>
    /// Checks for wallet toggle input & tracks private key held timer.
    /// </summary>
    private void Update()
    {
        CheckWalletToggleKeyInput();
        CheckHoldPrivateKeyButtonInput();
    }
    
    #endregion
}
