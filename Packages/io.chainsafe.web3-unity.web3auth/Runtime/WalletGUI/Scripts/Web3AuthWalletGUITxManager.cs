using System;
using ChainSafe.Gaming.UnityPackage;
using ChainSafe.GamingSdk.Web3Auth;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// Web3Auth wallet GUI transaction manager to handle transaction display & logic.
/// </summary>
public class Web3AuthWalletGUITxManager : MonoBehaviour
{
    #region Fields
    
    [SerializeField] private ScrollRect txScrollRect;
    [SerializeField] private TextMeshProUGUI incomingTxActionText;
    [SerializeField] private TextMeshProUGUI incomingTxHashText;
    [SerializeField] private GameObject txHistoryScrollPanel;
    [SerializeField] private GameObject txHistoryDataPrefab;
    [SerializeField] private GameObject incomingTxPlaceHolder;
    [SerializeField] private GameObject incomingTxDisplay;
    [SerializeField] private GameObject incomingTxNotification;
    [SerializeField] private GameObject txHistoryPlaceHolder;
    [SerializeField] private GameObject txHistoryDisplay;
    [SerializeField] private Button acceptRequestButton;
    [SerializeField] private Button rejectRequestButton;
    [SerializeField] private Web3AuthWalletGUI walletGui;
    [SerializeField] private Web3AuthWalletGUIUIManager guiManager;
    private GameObject[] txHistoryPrefabs;
    private int txObjectNumber = 1;
    private int txHistoryDisplayCount = 20;
    
    #endregion

    #region Properties

    private bool hasTransactionCompleted { get; set; }
    public bool AutoPopUpWalletOnTx { get; set; }
    public bool AutoConfirmTransactions { get; set; }
    
    #endregion

    #region Methods
    
    /// <summary>
    /// Initializes objects.
    /// </summary>
    private void Awake()
    {
        txHistoryPrefabs = new GameObject[txHistoryDisplayCount];
        walletGui = GetComponent<Web3AuthWalletGUI>();
        SetupButton(acceptRequestButton, AcceptRequest);
        SetupButton(rejectRequestButton, RejectRequest);
    }
    
    /// <summary>
    /// Populates the incoming transaction display.
    /// </summary>
    private void DisplayIncomingTransaction()
    {
        var data = Web3AuthTransactionHelper.StoredTransactionRequest;

        incomingTxNotification.SetActive(true);

        if (AutoConfirmTransactions)
        {
            AcceptRequest();
            return;
        }

        if (AutoPopUpWalletOnTx)
        {
            guiManager.ToggleWallet();
        }

        incomingTxPlaceHolder.SetActive(false);
        incomingTxDisplay.SetActive(true);
        incomingTxHashText.text = data.Data;
        incomingTxActionText.text = data.Value?.ToString() ?? "Sign Request";
    }
    
    /// <summary>
    /// Accepts an incoming transaction request.
    /// </summary>
    private void AcceptRequest()
    {
        ShowTxLoadingMenu();
        Web3AuthTransactionHelper.TransactionAccepted.Invoke();
        GetTransactionData();
    }
    
    /// <summary>
    /// Rejects an incoming transaction request.
    /// </summary>
    private void RejectRequest()
    {
        Web3AuthTransactionHelper.TransactionRejected.Invoke();
        ResetTransactionDisplay();
    }
    
    /// <summary>
    /// Gets transaction data.
    /// </summary>
    private async void GetTransactionData()
    {
        var requestData = Web3AuthTransactionHelper.StoredTransactionRequest;
        while (Web3AuthTransactionHelper.StoredTransactionResponse == null) await new WaitForSeconds(1);
        var txHash = Web3AuthTransactionHelper.StoredTransactionResponse.Hash;
        var txTime = DateTime.Now.ToString("hh:mm tt");
        var txAmount = requestData.Value?.ToString() ?? "0";
        var txAction = requestData.Value != null ? requestData.Value.ToString() : "Sign Request";
        AddTransactionToHistory(txTime, txAction, txAmount, txHash);
        ResetTransactionDisplay();
    }
    
    /// <summary>
    /// Adds a transaction to the history area.
    /// </summary>
    /// <param name="time">Transaction time</param>
    /// <param name="action">Action being performed</param>
    /// <param name="amount">Amount of tokens sent</param>
    /// <param name="txHash">Transaction hash</param>
    private void AddTransactionToHistory(string time, string action, string amount, string txHash)
    {
        txHistoryPlaceHolder.SetActive(false);
        txHistoryDisplay.SetActive(true);
        if (txObjectNumber >= txHistoryDisplayCount)
        {
            Destroy(txHistoryPrefabs[0]);
            for (int i = 1; i < txHistoryPrefabs.Length; i++)
            {
                txHistoryPrefabs[i - 1] = txHistoryPrefabs[i];
            }
            txHistoryPrefabs[txHistoryPrefabs.Length - 1] = Instantiate(txHistoryDataPrefab, txHistoryScrollPanel.transform);
            UpdateTransactionHistory(txHistoryPrefabs.Length - 1, txObjectNumber.ToString(), time, action, amount, txHash);
        }
        else
        {
            txHistoryPrefabs[txObjectNumber] = Instantiate(txHistoryDataPrefab, txHistoryScrollPanel.transform);
            UpdateTransactionHistory(txObjectNumber, txObjectNumber.ToString(), time, action, amount, txHash);
        }
        txObjectNumber++;
        txScrollRect.verticalNormalizedPosition = 0;
        ResetTransactionDisplay();
    }
    
    /// <summary>
    /// Resets the transaction display.
    /// </summary>
    private void ResetTransactionDisplay()
    {
        incomingTxNotification.SetActive(false);
        incomingTxActionText.text = string.Empty;
        incomingTxHashText.text = string.Empty;
        incomingTxDisplay.SetActive(false);
        incomingTxPlaceHolder.SetActive(true);
        Web3AuthTransactionHelper.StoredTransactionRequest = null;
        Web3AuthTransactionHelper.StoredTransactionResponse = null;
        hasTransactionCompleted = false;
    }
    
    /// <summary>
    /// Updates the transaction history prefab text.
    /// </summary>
    /// <param name="txObjectIndex">Position in the object array</param>
    /// <param name="txNumber">Transaction number</param>
    /// <param name="time">Transaction time</param>
    /// <param name="action">Action being performed</param>
    /// <param name="amount">Amount of tokens sent</param>
    /// <param name="txHash">Transaction hash</param>
    private void UpdateTransactionHistory(int txObjectIndex, string txNumber, string time, string action, string amount, string txHash)
    {
        string[] textObjectNames = { "TxNumberText", "TimeText", "ActionText", "AmountText", "TxHashText" };
        string[] textValues = { txNumber, time, action, amount, txHash };
        for (int i = 0; i < textObjectNames.Length; i++)
        {
            var textObj = txHistoryPrefabs[txObjectIndex].transform.Find(textObjectNames[i]);
            var textMeshPro = textObj.GetComponent<TextMeshProUGUI>();
            textMeshPro.text = textValues[i];
            textMeshPro.font = walletGui.DisplayFont;
            textMeshPro.color = walletGui.SecondaryTextColour;
        }

        var txHistoryObj = txHistoryPrefabs[txObjectIndex].transform.Find("ExplorerButton");
        // Disable block explorer button if no tx hash is returned.
        if (txHash == null) { txHistoryObj.gameObject.SetActive(false); return; }
        txHistoryObj.GetComponent<Button>().onClick.AddListener(() => OpenBlockExplorer(txHash));
        txHistoryObj.GetComponent<Image>().color = walletGui.SecondaryTextColour;
    }
    
    /// <summary>
    /// Opens the block explorer url.
    /// </summary>
    /// <param name="txHash"></param>
    private void OpenBlockExplorer(string txHash)
    {
        Application.OpenURL($"{Web3Accessor.Web3.ChainConfig.BlockExplorerUrl}/tx/{txHash}");
    }
    
    /// <summary>
    /// Shows the Tx loading menu
    /// </summary>
    private void ShowTxLoadingMenu()
    {
        incomingTxPlaceHolder.SetActive(true);
        incomingTxDisplay.SetActive(false);
    }
    
    /// <summary>
    /// Polls for incoming transactions & triggers display.
    /// </summary>
    private void CheckForIncomingTransaction()
    {
        if (Web3AuthTransactionHelper.TransactionAcceptedTcs == null) return;
        if (hasTransactionCompleted) return;
        Debug.Log("Incoming wallet transaction");
        hasTransactionCompleted = true;
        DisplayIncomingTransaction();
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
    /// Polls for incoming transactions.
    /// </summary>
    private void Update()
    {
        CheckForIncomingTransaction();
    }
    
    #endregion
}
