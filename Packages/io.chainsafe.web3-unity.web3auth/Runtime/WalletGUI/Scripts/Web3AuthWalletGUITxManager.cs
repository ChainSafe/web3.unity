using System;
using System.Collections.Generic;
using ChainSafe.Gaming.Evm.Transactions;
using ChainSafe.Gaming.UnityPackage;
using ChainSafe.GamingSdk.Web3Auth;
using Microsoft.Extensions.DependencyInjection;
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
    [SerializeField] private Toggle autoTxToggle;
    private GameObject[] txHistoryPrefabs;
    private int txObjectNumber = 1;
    private int txHistoryDisplayCount = 20;

    private bool _processingTransaction;
    private IWeb3AuthTransactionHandler _transactionHandler;
    private readonly Queue<TransactionRequest> _transactionQueue = new();

    #endregion

    #region Properties

    private bool AutoPopUpWalletOnTx { get; set; }
    private bool AutoConfirmTransactions { get; set; }
    private TMP_FontAsset DisplayFont { get; set; }
    private Color SecondaryTextColour { get; set; }

    #endregion

    #region Methods

    /// <summary>
    /// Initializes objects.
    /// </summary>
    private void Awake()
    {
        txHistoryPrefabs = new GameObject[txHistoryDisplayCount];
        acceptRequestButton.onClick.AddListener(AcceptRequest);
        rejectRequestButton.onClick.AddListener(RejectRequest);
        autoTxToggle.onValueChanged.AddListener(ToggleAutoTx);

        _transactionHandler = Web3Unity.Web3.ServiceProvider.GetService<IWeb3AuthTransactionHandler>();
    }

    /// <summary>
    /// Populates the incoming transaction display.
    /// </summary>
    private void OnTransactionRequested(TransactionRequest request)
    {
        _transactionQueue.Enqueue(request);

        if (_processingTransaction)
        {
            return;
        }

        _processingTransaction = true;

        PromptTransactionRequest();
    }

    /// <summary>
    /// Prompts transaction request display.
    /// </summary>
    private void PromptTransactionRequest()
    {
        var transaction = _transactionQueue.Peek();

        incomingTxNotification.SetActive(true);

        if (AutoConfirmTransactions)
        {
            AcceptRequest();

            return;
        }

        if (AutoPopUpWalletOnTx)
        {
            Web3AuthEventManager.RaiseToggleWallet();
        }

        incomingTxPlaceHolder.SetActive(false);
        incomingTxDisplay.SetActive(true);
        incomingTxHashText.text = transaction.Data;
        incomingTxActionText.text = transaction.Value?.ToString() ?? "Sign Request";
    }

    /// <summary>
    /// Accepts an incoming transaction request.
    /// </summary>
    private void AcceptRequest()
    {
        var transaction = _transactionQueue.Dequeue();
        ShowTxLoadingMenu();
        _transactionHandler.TransactionApproved(transaction.Id);
        ResetTransactionDisplay();
    }

    /// <summary>
    /// Rejects an incoming transaction request.
    /// </summary>
    private void RejectRequest()
    {
        var transaction = _transactionQueue.Dequeue();
        _transactionHandler.TransactionDeclined(transaction.Id);
        ResetTransactionDisplay();
    }

    /// <summary>
    /// Gets transaction data.
    /// </summary>
    private void OnTransactionConfirmed(TransactionResponse response)
    {
        var txHash = response.Hash;
        var txTime = DateTime.Now.ToString("hh:mm tt");
        var txAmount = response.Value?.ToString() ?? "0";
        var txAction = response.Value != null ? response.Value.ToString() : "Sign Request";
        AddTransactionToHistory(txTime, txAction, txAmount, txHash);
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
        // there's transactions in queue
        if (_transactionQueue.Count > 0)
        {
            PromptTransactionRequest();
        }

        else
        {
            _processingTransaction = false;
        }
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
            textMeshPro.font = DisplayFont;
            textMeshPro.color = SecondaryTextColour;
        }

        var txHistoryObj = txHistoryPrefabs[txObjectIndex].transform.Find("ExplorerButton");
        // Disable block explorer button if no tx hash is returned.
        if (txHash == null) { txHistoryObj.gameObject.SetActive(false); return; }
        txHistoryObj.GetComponent<Button>().onClick.AddListener(() => OpenBlockExplorer(txHash));
        txHistoryObj.GetComponent<Image>().color = SecondaryTextColour;
    }

    /// <summary>
    /// Opens the block explorer url.
    /// </summary>
    /// <param name="txHash"></param>
    private void OpenBlockExplorer(string txHash)
    {
        Application.OpenURL($"{Web3Unity.Web3.ChainConfig.BlockExplorerUrl}/tx/{txHash}");
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
    /// Toggles auto transactions.
    /// </summary>
    /// <param name="arg0"></param>
    private void ToggleAutoTx(bool arg0)
    {
        AutoConfirmTransactions = arg0;
    }

    /// <summary>
    /// Subscribes to events.
    /// </summary>
	private void OnEnable()
    {
        Web3AuthEventManager.ConfigureTxManager += OnConfigureTxManager;
        _transactionHandler.OnTransactionRequested += OnTransactionRequested;
        _transactionHandler.OnTransactionConfirmed += OnTransactionConfirmed;
    }

    /// <summary>
    /// Unsubscribes from events.
    /// </summary>
    private void OnDisable()
    {
        Web3AuthEventManager.ConfigureTxManager -= OnConfigureTxManager;
        _transactionHandler.OnTransactionRequested -= OnTransactionRequested;
        _transactionHandler.OnTransactionConfirmed -= OnTransactionConfirmed;
    }

    /// <summary>
    /// Configures class properties.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void OnConfigureTxManager(object sender, TxManagerConfigEventArgs args)
    {
        AutoPopUpWalletOnTx = args.AutoPopUpWalletOnTx;
        DisplayFont = args.DisplayFont;
        SecondaryTextColour = args.SecondaryTextColour;
        AutoConfirmTransactions = args.AutoConfirmTransactions;
        autoTxToggle.isOn = AutoConfirmTransactions;
    }

    #endregion
}
