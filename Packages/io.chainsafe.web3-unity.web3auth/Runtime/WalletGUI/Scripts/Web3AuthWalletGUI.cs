using System;
using System.Collections.Generic;
using ChainSafe.Gaming.UnityPackage;
using ChainSafe.GamingSdk.Web3Auth;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Web3Auth wallet GUI main class to manage initialization.
/// </summary>
public class Web3AuthWalletGUI : MonoBehaviour
{
    #region Fields
    
    [SerializeField] private Web3AuthWalletGUITxManager txManager;
    [SerializeField] private Web3AuthWalletGUIUIManager guiManager;
    [SerializeField] private Button acceptRequestButton;
    [SerializeField] private Button rejectRequestButton;
    [SerializeField] private GameObject walletIconContainer;
    [SerializeField] private List<GameObject> primaryBackgroundObjects;
    [SerializeField] private List<GameObject> menuBackgroundObjects;
    [SerializeField] private List<GameObject> primaryTextObjects;
    [SerializeField] private List<GameObject> secondaryTextObjects;

    #endregion

    #region Properties

    private bool DisplayWalletIcon { get; set; }
    private bool AutoPopUpWalletOnTx { get; set; }
    private bool AutoConfirmTransactions { get; set; }
    private Sprite WalletLogo { get; set; }
    private Color PrimaryBackgroundColour { get; set; }
    private Color MenuBackgroundColour { get; set; }
    private Color PrimaryTextColour { get; set; }
    public Color SecondaryTextColour { get; private set; }

    #endregion

    #region Methods
    
    /// <summary>
    /// Method to initialize parameters after prefab creation
    /// </summary>
    /// <param name="config">Web3Auth wallet configuration</param>
    public void Initialize(Web3AuthWalletConfig config)
    {
        DontDestroyOnLoad(gameObject);
        DisplayWalletIcon = config.DisplayWalletIcon;
        AutoPopUpWalletOnTx = config.AutoPopUpWalletOnTx;
        AutoConfirmTransactions = config.AutoConfirmTransactions;
        WalletLogo = config.WalletLogo;
        PrimaryBackgroundColour = config.PrimaryBackgroundColour;
        MenuBackgroundColour = config.MenuBackgroundColour;
        PrimaryTextColour = config.PrimaryTextColour;
        SecondaryTextColour = config.SecondaryTextColour;
        SetCustomConfig();
    }
    
    /// <summary>
    /// Sets custom config from the login scene
    /// </summary>
    private void SetCustomConfig()
    {
        walletIconContainer.SetActive(DisplayWalletIcon);
        txManager.AutoPopUpWalletOnTx = AutoPopUpWalletOnTx;
        txManager.AutoConfirmTransactions = AutoConfirmTransactions;
        guiManager.DisplayWalletIcon = DisplayWalletIcon;
        guiManager.walletLogoDisplay.GetComponent<Image>().sprite = WalletLogo;
        SetupButton(acceptRequestButton, AcceptRequest);
        SetupButton(rejectRequestButton, RejectRequest);
        SetCustomColours();
    }
    
    /// <summary>
    /// Utility method to amend button listeners
    /// </summary>
    /// <param name="button">Button to amend</param>
    /// <param name="action">Action to add</param>
    private void SetupButton(Button button, UnityEngine.Events.UnityAction action)
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(action);
    }
    
    /// <summary>
    /// Sets custom colours for menu and text objects
    /// </summary>
    private void SetCustomColours()
    {
        var objectsAndColours = new List<(List<GameObject> objects, Color color)>
        {
            (primaryBackgroundObjects, PrimaryBackgroundColour),
            (menuBackgroundObjects, MenuBackgroundColour),
            (primaryTextObjects, PrimaryTextColour),
            (secondaryTextObjects, SecondaryTextColour)
        };

        foreach (var (objects, color) in objectsAndColours)
        {
            foreach (var item in objects)
            {
                var imageRenderer = item.GetComponent<Image>();
                if (imageRenderer != null)
                {
                    imageRenderer.color = color;
                }
                var textMeshPro = item.GetComponent<TextMeshProUGUI>();
                if (textMeshPro != null)
                {
                    textMeshPro.font.material.color = color;
                }
            }
        }
    }

    
    /// <summary>
    /// Accepts an incoming transaction request.
    /// </summary>
    public async void AcceptRequest()
    {
        Web3AuthTransactionHelper.AcceptTransaction();
        await new WaitForSeconds(2);
        var txExecutor = (Web3AuthWallet)Web3Accessor.Web3.ServiceProvider.GetService(typeof(Web3AuthWallet));
        var requestData = await txExecutor.TransactionRequestTcs.Task;
        var txHash = await txExecutor.TransactionResponseTcs.Task;
        var txTime = DateTime.Now.ToString("hh:mm tt");
        var txAmount = requestData.Value?.ToString() ?? "0";
        var txAction = requestData.Value != null ? requestData.Value.ToString() : "Sign Request";
        txManager.AddTransaction(txTime, txAction, txAmount, txHash);
        txManager.ResetTransactionDisplay();
    }
    
    /// <summary>
    /// Rejects an incoming transaction request.
    /// </summary>
    private async void RejectRequest()
    {
        Web3AuthTransactionHelper.RejectTransaction();
        await new WaitForSeconds(2);
        txManager.ResetTransactionDisplay();
    }
    
    #endregion
    
    #region ConfigClass
    
    [Serializable]
    public class Web3AuthWalletConfig
    {
        public bool DisplayWalletIcon;
        public bool AutoPopUpWalletOnTx;
        public bool AutoConfirmTransactions;
        public Sprite WalletLogo;
        public Color PrimaryBackgroundColour;
        public Color MenuBackgroundColour;
        public Color PrimaryTextColour;
        public Color SecondaryTextColour;
    }
    
    #endregion
}
