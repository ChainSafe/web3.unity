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
        
        // Chain ID might be coming from the IChainConfig. But you need 2 of them. Source and Destination.
        // Probably this needs to be implemented in on the Dependency Injection level.
        uint destinationChainId = 305;
        
        // ResourceID should be coming from the Sygma resource registration process.
        // As soon as you register your 1155 or 721 in the Bridge, the appropriate resource id will be assigned.
        
        // Source Address is the address of the user who is sending the token. Basically Signer.GetAddress()
        
        // Destination Address is the address of the user who is receiving the token.
        // This data will be encoded into execution data of the passed cross-chain transaction.
        // To understand how it will be used check:
        // SigmaClient::CreateErc721DepositData and SigmaClient::CreateErc1155DepositData methods.
        
        // Token ID is a unique identifier of the token (NFT) you want to transfer.
 
        var transfer = await _sygmaClient.CreateNonFungibleTransfer(NonFungibleTransferType.Erc1155, "", destinationChainId, "", "", "");
        var fee = await _sygmaClient.Fee(transfer);
        var approvals = await _sygmaClient.BuildApprovals(transfer, fee, "");
        var transferTransaction = await _sygmaClient.BuildTransferTransaction(transfer, fee);
        var transactionHash = await Web3Accessor.Web3.TransactionExecutor.SendTransaction(transferTransaction);
    }
}