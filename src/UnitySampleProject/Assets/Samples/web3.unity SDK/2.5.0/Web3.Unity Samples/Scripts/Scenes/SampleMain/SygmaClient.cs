using ChainSafe.Gaming.SygmaClient;
using ChainSafe.Gaming.SygmaClient.Types;
using ChainSafe.Gaming.UnityPackage;
using UnityEngine;
using UnityEngine.UI;

public class SygmaClient : MonoBehaviour
{
    [SerializeField] private string chain;
    [SerializeField] private string resourceId;
    [SerializeField] private string somethingElse;
    [SerializeField] private Environment sygmaEnvironment = Environment.Testnet;
    [SerializeField] private Button button;

    private ISygmaClient _sygmaClient;
    private bool _isSygmaInitialized;

    private void Awake()
    {
        button.onClick.AddListener(CallSygma);
        _sygmaClient = Web3Accessor.Web3.SygmaClient();
        _isSygmaInitialized = _sygmaClient.Initialize(sygmaEnvironment);
        if (!_isSygmaInitialized)
        {
            Debug.LogError("Sygma failed to initialized");
        }
    }

    private async void CallSygma()
    {
        if (!_isSygmaInitialized)
        {
            Debug.LogError("Can't call Sygma if it's not initialized");
            return;
        }

        var transfer = await _sygmaClient.CreateNonFungibleTransfer(NonFungibleTransferType.Erc1155, "", 338, "", "", "");
        var fee = await _sygmaClient.Fee(transfer);
        var approvals = await _sygmaClient.BuildApprovals(transfer, fee, "");
        var transferTransaction = await _sygmaClient.BuildTransferTransaction(transfer, fee);
        var transactionHash = await Web3Accessor.Web3.TransactionExecutor.SendTransaction(transferTransaction);
    }
}