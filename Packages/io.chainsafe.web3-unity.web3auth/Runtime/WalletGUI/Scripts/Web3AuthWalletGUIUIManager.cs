using System.Collections;
using System.Collections.Generic;
using ChainSafe.Gaming.InProcessSigner;
using ChainSafe.Gaming;
using ChainSafe.Gaming.UnityPackage;
using ChainSafe.GamingSdk.Web3Auth;
using Nethereum.Web3.Accounts;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Web3Auth wallet GUI manager to handle UI elements.
/// </summary>
public class Web3AuthWalletGUIUIManager : MonoBehaviour
{
    #region Fields

    [SerializeField] private GameObject openWalletGUIContainer;
    [SerializeField] private GameObject walletGUIContainer;
    [SerializeField] private GameObject privateKeyContainer;
    [SerializeField] public GameObject walletIconDisplay;
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
    private ScreenOrientation originalOrientation;
    public float holdDuration = 2.0f;
    private Coroutine holdCoroutine;

    #endregion

    #region Properties

    private bool DisplayWalletIcon { get; set; }

    #endregion

    #region Methods

    /// <summary>
    /// Initializes objects.
    /// </summary>
    private void Start()
    {
        if (Web3Unity.Web3 == null)
        {
            Debug.LogError("Web3 instance not set.");

            gameObject.SetActive(false);

            return;
        }

        InitializeButtons();
        originalOrientation = Screen.orientation;
        walletAddressText.text = Web3Unity.Web3.Signer.PublicAddress;
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
    }

    /// <summary>
    /// Toggles the wallet display.
    /// </summary>
    private void ToggleWallet()
    {
        walletGUIContainer.SetActive(!walletGUIContainer.activeSelf);
        if (DisplayWalletIcon)
        {
            openWalletGUIContainer.SetActive(!openWalletGUIContainer.activeSelf);
        }

        if (!walletGUIContainer.activeSelf)
        {
            SetOriginalOrientation();
        }
        else
        {
            SetLandscapeOrientation();
        }
        Web3AuthEventManager.RaiseSetTokens();
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
        ClipboardManager.CopyText(walletAddressText.text);
    }

    /// <summary>
    /// Reveals the private key when held down.
    /// </summary>
    private void RevealPrivateKey()
    {
        SetPrivateKey();
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
            circleLoadingImage.fillAmount = 0;
            holdToRevealPrivateKeyButton.gameObject.SetActive(true);
            copyPrivateKeyButton.gameObject.SetActive(false);
            privateKeyText.gameObject.SetActive(false);
        }
        privateKeyContainer.SetActive(!privateKeyContainer.activeSelf);
    }

    /// <summary>
    /// On pointer down check.
    /// </summary>
    public void OnPointerDown()
    {
        holdCoroutine = StartCoroutine(CheckHoldPrivateKeyButtonInput());
    }

    /// <summary>
    /// On pointer up check.
    /// </summary>
    public void OnPointerUp()
    {
        if (circleLoadingImage.fillAmount >= 1.0f)
        {
            RevealPrivateKey();
        }

        if (holdCoroutine != null)
        {
            StopCoroutine(holdCoroutine);
            holdCoroutine = null;
        }

        circleLoadingImage.fillAmount = 0f;
    }

    /// <summary>
    /// IEnumerator for private key check.
    /// </summary>
    /// <returns></returns>
    private IEnumerator CheckHoldPrivateKeyButtonInput()
    {
        circleLoadingImage.fillAmount = 0f;

        while (circleLoadingImage.fillAmount < 1.0f)
        {
            circleLoadingImage.fillAmount += Time.deltaTime / holdDuration;
            circleLoadingImage.fillAmount = circleLoadingImage.fillAmount;
            yield return null;
        }

        OnPointerUp();
    }


    /// <summary>
    /// Sets the private key text.
    /// </summary>
    private void SetPrivateKey()
    {
        var accountProvider = (IAccountProvider)Web3Unity.Web3.ServiceProvider.GetService(typeof(IAccountProvider));

        privateKeyText.text = ((Account)accountProvider.Account).PrivateKey;
    }

    /// <summary>
    /// Copies the wallet private key to clipboard.
    /// </summary>
    private void CopyPrivateKey()
    {
        ClipboardManager.CopyText(privateKeyText.text);
    }

    /// <summary>
    /// Sets original orientation
    /// </summary>
    private void SetOriginalOrientation()
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android)
        {
            Screen.orientation = originalOrientation;
        }
    }

    /// <summary>
    /// Sets landscape origin for mobile devices when opened.
    /// </summary>
    private void SetLandscapeOrientation()
    {
        // Set landscape orientation based on platform
        if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android)
        {
            Screen.orientation = ScreenOrientation.LandscapeLeft;
        }
    }

    /// <summary>
    /// Subscribes to events.
    /// </summary>
    private void OnEnable()
    {
        Web3AuthEventManager.ConfigureGuiManager += OnConfigureGuiManager;
        Web3AuthEventManager.ToggleWallet += ToggleWallet;
    }

    /// <summary>
    /// Unsubscribes from events.
    /// </summary>
    private void OnDisable()
    {
        privateKeyText.text = "";
        Web3AuthEventManager.ConfigureGuiManager -= OnConfigureGuiManager;
        Web3AuthEventManager.ToggleWallet -= ToggleWallet;
    }

    /// <summary>
    /// Configures class properties.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void OnConfigureGuiManager(object sender, GuiManagerConfigEventArgs args)
    {
        DisplayWalletIcon = args.DisplayWalletIcon;
        walletIconDisplay.GetComponent<Image>().sprite = args.WalletIcon;
        walletLogoDisplay.GetComponent<Image>().sprite = args.WalletLogo;
    }

    /// <summary>
    /// Checks for wallet toggle input & tracks private key held timer.
    /// </summary>
    private void Update()
    {
        CheckWalletToggleKeyInput();
    }

    #endregion
}
