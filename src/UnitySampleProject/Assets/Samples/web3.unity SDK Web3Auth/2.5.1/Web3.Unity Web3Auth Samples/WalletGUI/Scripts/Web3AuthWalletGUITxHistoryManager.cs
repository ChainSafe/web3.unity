using ChainSafe.Gaming.UnityPackage;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Web3AuthWalletGUITxHistoryManager : MonoBehaviour
{
    [SerializeField] private Toggle autoTxToggle;
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
    private GameObject[] txHistoryPrefabs;
    private Web3AuthWalletGUI w3aWalletGUI;
    private int txObjectNumber = 1;
    private int txHistoryDisplayCount = 20;

    private void Awake()
    {
        txHistoryPrefabs = new GameObject[txHistoryDisplayCount];
        w3aWalletGUI = GetComponent<Web3AuthWalletGUI>();
    }
    
    private void IncomingTransactionDisplay(string action, string txHash)
    {
        incomingTxNotification.SetActive(true);
        if (autoTxToggle)
        {
            w3aWalletGUI.AcceptRequest();
            return;
        }
        incomingTxPlaceHolder.SetActive(false);
        incomingTxDisplay.SetActive(true);
        incomingTxActionText.text = action;
        incomingTxHashText.text = txHash;
    }

    public void AddTransaction(string time, string action, string amount, string txHash)
    {
        if (txObjectNumber >= txHistoryDisplayCount)
        {
            Destroy(txHistoryPrefabs[0]);
            for (int i = 1; i < txHistoryPrefabs.Length; i++)
            {
                txHistoryPrefabs[i - 1] = txHistoryPrefabs[i];
            }
            txHistoryPrefabs[txHistoryPrefabs.Length - 1] = Instantiate(txHistoryDataPrefab, txHistoryScrollPanel.transform);
            UpdateTxHistoryObject(txHistoryPrefabs.Length - 1, txObjectNumber.ToString(), time, action, amount, txHash);
        }
        else
        {
            txHistoryPrefabs[txObjectNumber] = Instantiate(txHistoryDataPrefab, txHistoryScrollPanel.transform);
            UpdateTxHistoryObject(txObjectNumber, txObjectNumber.ToString(), time, action, amount, txHash);
        }
        txObjectNumber++;
        txScrollRect.verticalNormalizedPosition = 0;
        ResetTransactionDisplay();
    }
    
    public void ResetTransactionDisplay()
    {
        incomingTxNotification.SetActive(false);
        if (autoTxToggle) return;
        incomingTxActionText.text = string.Empty;
        incomingTxHashText.text = string.Empty;
        incomingTxDisplay.SetActive(false);
        incomingTxPlaceHolder.SetActive(true);
    }

    private void UpdateTxHistoryObject(int txObjectIndex, string txNumber, string time, string action, string amount, string txHash)
    {
        txHistoryPrefabs[txObjectIndex].transform.Find("TxNumberText").GetComponent<TextMeshProUGUI>().text = $"#{txNumber}";
        txHistoryPrefabs[txObjectIndex].transform.Find("TimeText").GetComponent<TextMeshProUGUI>().text = time;
        txHistoryPrefabs[txObjectIndex].transform.Find("ActionText").GetComponent<TextMeshProUGUI>().text = action;
        txHistoryPrefabs[txObjectIndex].transform.Find("AmountText").GetComponent<TextMeshProUGUI>().text = amount;
        txHistoryPrefabs[txObjectIndex].transform.Find("TxHashText").GetComponent<TextMeshProUGUI>().text = txHash;
        txHistoryPrefabs[txObjectIndex].transform.Find("ExplorerButton").GetComponent<Button>().onClick.AddListener(() => OpenBlockExplorer(txHash));
    }

    private void OpenBlockExplorer(string txHash)
    {
        Debug.Log($"Opening URL: {Web3Accessor.Web3.ChainConfig.BlockExplorerUrl}/{txHash}");
        Application.OpenURL($"{Web3Accessor.Web3.ChainConfig.BlockExplorerUrl}/{txHash}");
    }
}
