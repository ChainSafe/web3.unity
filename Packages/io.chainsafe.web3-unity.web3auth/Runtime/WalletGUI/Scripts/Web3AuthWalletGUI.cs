using System;
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
    [SerializeField] private Sprite walletLogo;
    [SerializeField] private GameObject walletIconContainer;
    [SerializeField] private bool displayWalletIcon;
    [SerializeField] private bool autoPopUpWalletOnTx;

    #endregion

    #region Methods
    
    /// <summary>
    /// Initializes objects.
    /// </summary>
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        walletIconContainer.SetActive(displayWalletIcon);
        txManager.autoPopUpWalletOnTx = autoPopUpWalletOnTx;
        guiManager.walletLogoDisplay.GetComponent<Image>().sprite = walletLogo;
        acceptRequestButton.onClick.AddListener(AcceptRequest);
        rejectRequestButton.onClick.AddListener(RejectRequest);
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
