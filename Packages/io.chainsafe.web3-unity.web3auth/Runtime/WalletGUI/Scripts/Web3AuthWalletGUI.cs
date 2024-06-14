using System;
using System.Collections.Generic;
using ChainSafe.Gaming.InProcessTransactionExecutor;
using ChainSafe.Gaming.UnityPackage;
using ChainSafe.GamingSdk.Web3Auth;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Web3AuthWalletGUI : MonoBehaviour
{
    #region Fields
    
    [SerializeField] private Web3AuthWalletGUITxManager txManager;
    [SerializeField] private Web3AuthWalletGUIUIManager guiManager;
    [SerializeField] private TextMeshProUGUI incomingTxActionText;
    [SerializeField] private TextMeshProUGUI incomingTxHashText;
    [SerializeField] private Button acceptRequestButton;
    [SerializeField] private Button rejectRequestButton;
    [SerializeField] private GameObject walletIconContainer;
    [SerializeField] private List<GameObject> primaryBackgroundObjects;
    [SerializeField] private List<GameObject> menuBackgroundObjects;
    [SerializeField] private List<GameObject> primaryTextObjects;
    [SerializeField] private List<GameObject> secondaryTextObjects;
    private bool displayWalletIcon;
    private bool autoPopUpWalletOnTx;
    private bool autoConfirmTransactions;
    private Sprite walletLogo;
    private Color primaryBackgroundColour;
    private Color menuBackgroundColour;
    private Color primaryTextColour;
    private Color secondaryTextColour;

    #endregion

    #region Methods
    
    /// <summary>
    /// Method to initialize parameters after prefab creation
    /// </summary>
    /// <param name="displayWalletIcon">If the wallet icon should be displayed or not</param>
    /// <param name="autoPopUpWalletOnTx">If the wallet should automatically pop up or not</param>
    /// <param name="autoConfirmTransactions">If transactions should be auto confirmed or not</param>
    /// <param name="walletLogo">The logo to display on the wallet</param>
    /// <param name="primaryBackgroundColour">Primary background colour</param>
    /// <param name="menuBackgroundColour">Menu background colour</param>
    /// <param name="primaryTextColour">Primary text colour</param>
    /// <param name="secondaryTextColour">Secondary text colour</param>
    public void Initialize(bool displayWalletIcon, bool autoPopUpWalletOnTx, bool autoConfirmTransactions, Sprite walletLogo, Color primaryBackgroundColour, Color menuBackgroundColour, Color primaryTextColour, Color secondaryTextColour)
    {
        DontDestroyOnLoad(gameObject);
        this.displayWalletIcon = displayWalletIcon;
        this.autoPopUpWalletOnTx = autoPopUpWalletOnTx;
        this.autoConfirmTransactions = autoConfirmTransactions;
        this.walletLogo = walletLogo;
        this.primaryBackgroundColour = primaryBackgroundColour;
        this.menuBackgroundColour = menuBackgroundColour;
        this.primaryTextColour = primaryTextColour;
        this.secondaryTextColour = secondaryTextColour;
        SetCustomConfig();
    }
    
    /// <summary>
    /// Sets custom config from the login scene
    /// </summary>
    private void SetCustomConfig()
    {
        walletIconContainer.SetActive(displayWalletIcon);
        txManager.autoPopUpWalletOnTx = autoPopUpWalletOnTx;
        txManager.autoConfirmTransactions = autoConfirmTransactions;
        guiManager.displayWalletIcon = displayWalletIcon;
        guiManager.walletLogoDisplay.GetComponent<Image>().sprite = walletLogo;
        acceptRequestButton.onClick.AddListener(AcceptRequest);
        rejectRequestButton.onClick.AddListener(RejectRequest);
        SetCustomColours(primaryBackgroundObjects, menuBackgroundObjects, primaryTextObjects, secondaryTextObjects);
    }
    
    /// <summary>
    /// Sets custom colours for menu and text objects
    /// </summary>
    /// <param name="primaryBackgroundObjects">Primary background objects</param>
    /// <param name="menuBackgroundObjects">Menu background objects</param>
    /// <param name="primaryTextObjects">Primary text objects</param>
    /// <param name="secondaryTextObjects">Secondary text objets</param>
    private void SetCustomColours(List<GameObject> primaryBackgroundObjects, List<GameObject> menuBackgroundObjects, List<GameObject> primaryTextObjects, List<GameObject> secondaryTextObjects)
    {
        // Create a list of tuples pairing objects with their colors
        var objectsAndColours = new List<(List<GameObject> objects, Color color)>
        {
            (primaryBackgroundObjects, primaryBackgroundColour),
            (menuBackgroundObjects, menuBackgroundColour),
            (primaryTextObjects, primaryTextColour),
            (secondaryTextObjects, secondaryTextColour)
        };

        // Iterate over the list of tuples and apply the colors
        foreach (var (objects, color) in objectsAndColours)
        {
            foreach (var item in objects)
            {
                item.GetComponent<Renderer>().material.color = color;
            }
        }
    }
    
    /// <summary>
    /// Accepts an incoming transaction request.
    /// </summary>
    public async void AcceptRequest()
    {
        Web3AuthTransactionHelper.AcceptTransaction();
        // short wait to stop null ref
        await new WaitForSeconds(2);
        var txExecutor = (Web3AuthWallet)Web3Accessor.Web3.ServiceProvider.GetService(typeof(Web3AuthWallet));
        var requestData = txExecutor.TransactionRequestTcs.Task.Result;
        var txHash = txExecutor.TransactionResponseTcs.Task.Result;
        var txTime = DateTime.Now.ToString("hh:mm: tt");
        var txAmount = requestData.Value == null ? "0" : requestData.Value.ToString();
        var txAction = incomingTxActionText.text = requestData.Value != null ? requestData.Value.ToString() : "Sign Request";
        txManager.AddTransaction(txTime, txAction, txAmount, txHash);
        txManager.ResetTransactionDisplay();
    }
    
    /// <summary>
    /// Rejects an incoming transaction request.
    /// </summary>
    private void RejectRequest()
    {
        txManager.ResetTransactionDisplay();
    }
    
    #endregion
}
