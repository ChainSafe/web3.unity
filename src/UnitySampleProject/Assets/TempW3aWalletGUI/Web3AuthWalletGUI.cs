using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using ChainSafe.Gaming.Evm.Contracts.BuiltIn;
using ChainSafe.Gaming.InProcessSigner;
using ChainSafe.Gaming.InProcessTransactionExecutor;
using ChainSafe.Gaming.UnityPackage;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Core.Evm;
using ChainSafe.GamingSdk.Web3Auth;
using Scripts.EVM.Token;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Web3AuthWalletGUI : MonoBehaviour
{

    #region Fields
    
    [SerializeField] private Toggle autoTxToggle;
    [SerializeField] private TextMeshProUGUI walletAddressText;
    [SerializeField] private TextMeshProUGUI privateKeyText;
    [SerializeField] private TextMeshProUGUI incomingTxActionText;
    [SerializeField] private TextMeshProUGUI incomingTxHashText;
    [SerializeField] private TextMeshProUGUI nativeTokenAmountText;
    [SerializeField] private TextMeshProUGUI nativeTokenSymbolText;
    [SerializeField] private TextMeshProUGUI customTokenAmountText;
    [SerializeField] private TextMeshProUGUI customTokenSymbolText;
    [SerializeField] private TMP_InputField customTokenAddressInput;
    [SerializeField] private TMP_InputField customTokenSymbolInput;
    [SerializeField] private TMP_InputField transferTokensWalletInput;
    [SerializeField] private TMP_InputField transferTokensAmountInput;
    [SerializeField] private TMP_Dropdown selectedTokenToTransfer;
    [SerializeField] private Button openWalletButton;
    [SerializeField] private Button closeWalletButton;
    [SerializeField] private Button copyWalletAddressButton;
    [SerializeField] private Button openPrivateKeyMenuButton;
    [SerializeField] private Button closePrivateKeyMenuButton;
    [SerializeField] private Button copyPrivateKeyButton;
    [SerializeField] private Button addTokensMenuButton;
    [SerializeField] private Button addTokenButton;
    [SerializeField] private Button closeAddTokensMenuButton;
    [SerializeField] private Button transferTokensMenuButton;
    [SerializeField] private Button transferTokensButton;
    [SerializeField] private Button closeTransferTokensButton;
    [SerializeField] private Button acceptRequestButton;
    [SerializeField] private Button rejectRequestButton;
    [SerializeField] private GameObject openWalletGUIContainer;
    [SerializeField] private GameObject walletGUIContainer;
    [SerializeField] private GameObject privateKeyContainer;
    [SerializeField] private GameObject transferTokensContainer;
    [SerializeField] private GameObject addCustomTokensContainer;
    [SerializeField] private GameObject customTokenPlaceHolder;
    [SerializeField] private GameObject customTokenDisplay;
    [SerializeField] private GameObject incomingTxPlaceHolder;
    [SerializeField] private GameObject incomingTxDisplay;
    [SerializeField] private GameObject incomingTxNotification;
    [SerializeField] private GameObject txHistoryPlaceHolder;
    [SerializeField] private GameObject txHistoryDisplay;
    [SerializeField] private GameObject txHistoryScrollPanel;
    [SerializeField] private GameObject txHistoryDataPrefab;
    [SerializeField] private int txHistoryDisplayCount = 20;
    [SerializeField] private ScrollRect txScrollRect;
    private GameObject[] TxHistoryPrefabs;
    private int txObjectNumber = 1;
    private string customTokenContract;

    #endregion

    #region Methods

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        openWalletButton.onClick.AddListener(ToggleWalletButton);
        closeWalletButton.onClick.AddListener(ToggleWalletButton);
        copyWalletAddressButton.onClick.AddListener(CopyWalletAddressButton);
        openPrivateKeyMenuButton.onClick.AddListener(TogglePrivateKeyMenuButton);
        closePrivateKeyMenuButton.onClick.AddListener(TogglePrivateKeyMenuButton);
        copyPrivateKeyButton.onClick.AddListener(CopyPrivateKeyButton);
        addTokensMenuButton.onClick.AddListener(ToggleAddTokensMenuButton);
        closeAddTokensMenuButton.onClick.AddListener(ToggleAddTokensMenuButton);
        addTokenButton.onClick.AddListener(AddTokenButton);
        transferTokensMenuButton.onClick.AddListener(ToggleTransferTokensMenuButton);
        closeTransferTokensButton.onClick.AddListener(ToggleTransferTokensMenuButton);
        transferTokensButton.onClick.AddListener(TransferTokens);
        acceptRequestButton.onClick.AddListener(AcceptRequest);
        rejectRequestButton.onClick.AddListener(RejectRequest);
        TxHistoryPrefabs = new GameObject[txHistoryDisplayCount];
        SetPrivateKey();
        SetTokens();
    }

    private void ToggleWalletButton()
    {
        if (walletGUIContainer.activeSelf)
        {
            walletGUIContainer.SetActive(false);
            openWalletGUIContainer.SetActive(true);
        }
        else
        {
            walletGUIContainer.SetActive(true);
            openWalletGUIContainer.SetActive(false);
        }
    }

    private void CopyWalletAddressButton()
    {
        GUIUtility.systemCopyBuffer = walletAddressText.text;
    }
    
    private void TogglePrivateKeyMenuButton()
    {
        privateKeyContainer.SetActive(!privateKeyContainer.activeSelf);
    }
    
    private void SetPrivateKey()
    {
        var w3a = (Web3AuthWallet)Web3Accessor.Web3.ServiceProvider.GetService(typeof(Web3AuthWallet));
        privateKeyText.text = w3a.Key;
    }

    private void SetTokens()
    {
        // Set custom token
        if (File.Exists(Path.Combine(Application.persistentDataPath, "customToken.txt")))
        {
            var customTokenData = File.ReadAllText(Path.Combine(Application.persistentDataPath, "customToken.txt"));
            var data = customTokenData.Split(",");
            customTokenContract = data[0];
            customTokenSymbolText.text = data[1];
        }
        // Set native token
        nativeTokenSymbolText.text = Web3Accessor.Web3.ChainConfig.Symbol;
    }

    private void CopyPrivateKeyButton()
    {
        GUIUtility.systemCopyBuffer = privateKeyText.text;
    }
    
    private void ToggleAddTokensMenuButton()
    {
        addCustomTokensContainer.SetActive(!addCustomTokensContainer.activeSelf);
    }
    
    private void AddTokenButton()
    {
        if (!File.Exists(Path.Combine(Application.persistentDataPath, "customToken.txt")))
        {
            File.WriteAllText(Path.Combine(Application.persistentDataPath, "customToken.txt"), $"{customTokenAddressInput.text},{customTokenSymbolInput.text}");
        }
        customTokenSymbolText.text = customTokenSymbolInput.text;
        customTokenAddressInput.text = string.Empty;
        customTokenSymbolInput.text = string.Empty;
        addCustomTokensContainer.SetActive(false);
        customTokenPlaceHolder.SetActive(false);
        customTokenDisplay.SetActive(true);
    }
    
    private void ToggleTransferTokensMenuButton()
    {
        transferTokensContainer.SetActive(!transferTokensContainer.activeSelf);
    }
    
    private async void TransferTokens()
    {
        // Set token options
        if (selectedTokenToTransfer.options == null)
        {
            selectedTokenToTransfer.options = new List<TMP_Dropdown.OptionData>(2);
            selectedTokenToTransfer.options[0].text = nativeTokenSymbolText.text;
            selectedTokenToTransfer.options[1].text = customTokenSymbolText.text;
        }
        
        // Token transfers
        switch (selectedTokenToTransfer.value)
        {
            case 0:
                var amountInWei0 = BigInteger.Parse(transferTokensAmountInput.text) * (BigInteger)Math.Pow(10, 18);
                var response0 = await Evm.SendTransaction(Web3Accessor.Web3, transferTokensWalletInput.text, amountInWei0);
                SampleOutputUtil.PrintResult(response0, nameof(Evm), nameof(Evm.SendTransaction));
                break;
            case 1:
                var amountInWei1 = BigInteger.Parse(transferTokensAmountInput.text) * (BigInteger)Math.Pow(10, 18);
                var response1 = await Web3Accessor.Web3.Erc20.Transfer(customTokenContract, transferTokensWalletInput.text, amountInWei1);
                var output = SampleOutputUtil.BuildOutputValue(response1);
                SampleOutputUtil.PrintResult(output, "ERC-20", nameof(Erc20Service.Transfer));
                break;
            default:
                throw new Web3Exception("Token can't be found");
        }
    }

    private void IncomingTransaction()
    {
        //TODO to execute when txs are coming in
        var txe = (InProcessSigner)Web3Accessor.Web3.ServiceProvider.GetService(typeof(InProcessSigner));
        incomingTxNotification.SetActive(true);
        IncomingTransactionDisplay("action", "txHash");
    }

    private void IncomingTransactionDisplay(string action, string txHash)
    {
        if (autoTxToggle)
        {
            AcceptRequest();
            return;
        }
        incomingTxPlaceHolder.SetActive(false);
        incomingTxDisplay.SetActive(true);
        incomingTxActionText.text = action;
        incomingTxHashText.text = txHash;
    }
    
    private void AcceptRequest()
    {
        // TODO update vars below with tx data
        // Get transaction data
        var txTime = "";
        var txAction = "";
        var txAmount = "";
        var txHash = "";
        
        // Display transaction data
        // Appends tx history list & removes oldest entry if over transaction count for performance reasons
        if (txObjectNumber >= txHistoryDisplayCount)
        {
            // Destroy the oldest entry
            Destroy(TxHistoryPrefabs[0]);
            // Shift all elements down by one position
            for (int i = 1; i < TxHistoryPrefabs.Length; i++)
            {
                TxHistoryPrefabs[i - 1] = TxHistoryPrefabs[i];
            }
            // Create a new entry at the end of the array
            TxHistoryPrefabs[TxHistoryPrefabs.Length - 1] = Instantiate(txHistoryDataPrefab, txHistoryScrollPanel.transform);
            UpdateTxHistoryObject(TxHistoryPrefabs.Length - 1, txObjectNumber.ToString(), txTime, txAction, txAmount, txHash);
        }
        else
        {
            // Appends tx history list
            TxHistoryPrefabs[txObjectNumber] = Instantiate(txHistoryDataPrefab, txHistoryScrollPanel.transform);
            UpdateTxHistoryObject(txObjectNumber, txObjectNumber.ToString(), txTime, txAction, txAmount, txHash);
        }
        
        // Increase object counter and reset display
        txObjectNumber++;
        ResetTransactionDisplay();
    }
    
    private void RejectRequest()
    {
        ResetTransactionDisplay();
    }

    private void ResetTransactionDisplay()
    {
        txScrollRect.verticalNormalizedPosition = 0;
        incomingTxNotification.SetActive(false);
        if (autoTxToggle) return;
        incomingTxActionText.text = string.Empty;
        incomingTxHashText.text = string.Empty;
        incomingTxDisplay.SetActive(false);
        incomingTxPlaceHolder.SetActive(true);
    }
    
    private void UpdateTxHistoryObject(int txObjectIndex, string txNumber, string time, string action, string amount, string txHash)
    {
        TxHistoryPrefabs[txObjectIndex].transform.Find("TxNumberText").GetComponent<TextMeshProUGUI>().text = $"#{txNumber}";
        TxHistoryPrefabs[txObjectIndex].transform.Find("TimeText").GetComponent<TextMeshProUGUI>().text = time;
        TxHistoryPrefabs[txObjectIndex].transform.Find("ActionText").GetComponent<TextMeshProUGUI>().text = action;
        TxHistoryPrefabs[txObjectIndex].transform.Find("AmountText").GetComponent<TextMeshProUGUI>().text = amount;
        TxHistoryPrefabs[txObjectIndex].transform.Find("TxHashText").GetComponent<TextMeshProUGUI>().text = txHash;
        TxHistoryPrefabs[txObjectIndex].transform.Find("ExplorerButton").GetComponent<Button>().onClick.AddListener(() => OpenBlockExplorer(txHash));
    }
    
    private void OpenBlockExplorer(string txHash)
    {
        Debug.Log($"opening URL {Web3Accessor.Web3.ChainConfig.BlockExplorerUrl}/{txHash}");
        Application.OpenURL($"{Web3Accessor.Web3.ChainConfig.BlockExplorerUrl}/{txHash}");
    }
    
    private void WalletToggleKeyInputCheck()
    {
        // Check for shift + tab press to allow opening and closing of wallet GUI
        if ((Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)) && Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleWalletButton();
        }
    }
    
    private void Update()
    {
        WalletToggleKeyInputCheck();
    }
    
    #endregion
}
