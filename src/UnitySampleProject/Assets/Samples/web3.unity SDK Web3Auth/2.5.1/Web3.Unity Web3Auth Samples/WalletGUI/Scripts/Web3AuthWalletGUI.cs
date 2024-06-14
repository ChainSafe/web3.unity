using System;
using System.Numerics;
using System.Threading.Tasks;
using ChainSafe.Gaming.InProcessTransactionExecutor;
using ChainSafe.Gaming.UnityPackage;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Web3AuthWalletGUI : MonoBehaviour
{
    #region Fields
    
    [SerializeField] private TextMeshProUGUI incomingTxActionText;
    [SerializeField] private TextMeshProUGUI incomingTxHashText;
    [SerializeField] private Button acceptRequestButton;
    [SerializeField] private Button rejectRequestButton;
    private Web3AuthWalletGUITxManager txHistoryManager;
    public TaskCompletionSource<bool> TransactionAcceptedTcs { get; private set; }

    #endregion

    #region Methods

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        txHistoryManager = GetComponent<Web3AuthWalletGUITxManager>();
        acceptRequestButton.onClick.AddListener(AcceptRequest);
        rejectRequestButton.onClick.AddListener(RejectRequest);
        TransactionAcceptedTcs = new TaskCompletionSource<bool>();
    }
    
    public async void AcceptRequest()
    {
        Web3AuthTransactionHelper.AcceptTransaction();
        await Web3AuthTransactionHelper.WaitForTransactionAsync();
        var txExecutor = (InProcessTransactionExecutor)Web3Accessor.Web3.ServiceProvider.GetService(typeof(InProcessTransactionExecutor));
        var requestData = txExecutor.TransactionRequestTcs.Task.Result;
        var responseData = txExecutor.TransactionResponseTcs.Task.Result;
        // Get transaction data
        var txTime = DateTime.Now.ToString("hh:mm: tt");
        var txHash = responseData;
        var txAmount = requestData.Value;
        var txAction = requestData.Value == BigInteger.Parse("0") ? "Sign" : requestData.Value.ToString();;
        txHistoryManager.AddTransaction(txTime, txAction, txAmount.ToString(), txHash);
        txHistoryManager.ResetTransactionDisplay();
    }
    
    private void RejectRequest()
    {
        txHistoryManager.ResetTransactionDisplay();
    }
    
    #endregion
}
