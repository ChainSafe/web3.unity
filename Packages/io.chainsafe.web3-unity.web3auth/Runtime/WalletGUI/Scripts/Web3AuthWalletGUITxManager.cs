using System.Threading.Tasks;
using ChainSafe.Gaming.UnityPackage;
using ChainSafe.GamingSdk.Web3Auth;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

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
    [SerializeField] private Web3AuthWalletGUI walletGui;
    [SerializeField] private Web3AuthWalletGUIUIManager guiManager;
    private GameObject[] txHistoryPrefabs;
    private int txObjectNumber = 1;
    private int txHistoryDisplayCount = 20;
    
    #endregion

    #region Properties

    private bool hasTransactionCompleted { get; set; }
    public bool AutoPopUpWalletOnTx { get; set; }
    public TaskCompletionSource<bool> TransactionResponseTcs { get; private set; }
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
        TransactionResponseTcs = new TaskCompletionSource<bool>();
    }
    
    /// <summary>
    /// Populates the incoming transaction display.
    /// </summary>
    private void DisplayIncomingTransaction()
    {
        var web3AuthWallet = (Web3AuthWallet)Web3Accessor.Web3.ServiceProvider.GetService(typeof(Web3AuthWallet));
        var data = web3AuthWallet.TransactionRequestTcs.Task.Result;

        incomingTxNotification.SetActive(true);

        if (AutoConfirmTransactions)
        {
            walletGui.AcceptRequest();
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
    /// Adds a transaction to the history area.
    /// </summary>
    /// <param name="time">Transaction time</param>
    /// <param name="action">Action being performed</param>
    /// <param name="amount">Amount of tokens sent</param>
    /// <param name="txHash">Transaction hash</param>
    public void AddTransaction(string time, string action, string amount, string txHash)
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
    public void ResetTransactionDisplay()
    {
        incomingTxNotification.SetActive(false);
        incomingTxActionText.text = string.Empty;
        incomingTxHashText.text = string.Empty;
        incomingTxDisplay.SetActive(false);
        incomingTxPlaceHolder.SetActive(true);
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
            textMeshPro.material.color = walletGui.SecondaryTextColour;
        }
        txHistoryPrefabs[txObjectIndex].transform.Find("ExplorerButton").GetComponent<Button>().onClick.AddListener(() => OpenBlockExplorer(txHash));
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
    /// Polls for incoming transactions & triggers display.
    /// </summary>
    private void CheckForIncomingTransaction()
    {
        if (Web3AuthTransactionHelper.TransactionAcceptedTcs == null) return;

        if (!hasTransactionCompleted && Web3AuthTransactionHelper.Working)
        {
            Debug.Log("Incoming wallet transaction");
            hasTransactionCompleted = true;
            DisplayIncomingTransaction();
        }
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
