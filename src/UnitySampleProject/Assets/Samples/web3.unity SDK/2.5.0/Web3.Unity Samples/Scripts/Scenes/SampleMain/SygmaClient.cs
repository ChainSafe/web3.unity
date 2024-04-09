using System.Numerics;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Transactions;
using ChainSafe.Gaming.SygmaClient;
using ChainSafe.Gaming.SygmaClient.Types;
using ChainSafe.Gaming.UnityPackage;
using ChainSafe.Gaming.Web3;
using Scripts.EVM.Token;
using UnityEngine;
using UnityEngine.UI;

public class SygmaClient : MonoBehaviour
{
    [SerializeField] private string destinationChainId;
    [SerializeField] private string resourceId;
    [SerializeField] private Environment sygmaEnvironment = Environment.Testnet;
    [SerializeField] private Button button;

    private ISygmaClient _sygmaClient;
    private bool _isSygmaInitialized;
    private Web3 _web3;

    private void Awake()
    {
        button.onClick.AddListener(CallSygma);
        _web3 = Web3Accessor.Web3;
        _sygmaClient = Web3Accessor.Web3.SygmaClient();
        Debug.Log("Initializing Sygma...");
        _isSygmaInitialized = _sygmaClient.Initialize(sygmaEnvironment);
        Debug.Log("DONE!...");
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

        if(!uint.TryParse(destinationChainId, out var destination))
        {
            Debug.LogError("Invalid destination chain id");
            return;
        }
        // ResourceID should be coming from the Sygma resource registration process.
        // As soon as you register your 1155 or 721 in the Bridge, the appropriate resource id will be assigned.
        
        // Source Address is the address of the user who is sending the token. Basically Signer.GetAddress()
        
        // Destination Address is the address of the user who is receiving the token.
        // This data will be encoded into execution data of the passed cross-chain transaction.
        // To understand how it will be used check:
        // SigmaClient::CreateErc721DepositData and SigmaClient::CreateErc1155DepositData methods.
        
        // Token ID is a unique identifier of the token (NFT) you want to transfer.
        var address = await _web3.Signer.GetAddress();
        Debug.Log("Preparing transfer");
        var transfer = await _sygmaClient.CreateNonFungibleTransfer(NonFungibleTransferType.Erc1155,  address , destination, address, resourceId, "1");
        Debug.Log("Done transfer");
        SampleOutputUtil.PrintResult("Transfer created", "SygmaClient", "CreateNonFungibleTransfer");
        var fee = await _sygmaClient.Fee(transfer);
        Debug.Log("Done fee");
        SampleOutputUtil.PrintResult("Fee calculated", "SygmaClient", "Fee");
        var approvals = await _sygmaClient.BuildApprovals(transfer, fee, Contracts.Erc1155);
        Debug.Log("Approvals done");

        SampleOutputUtil.PrintResult("Approvals created", "SygmaClient", "BuildApprovals");
        var transferTransaction = await _sygmaClient.BuildTransferTransaction(transfer, fee);
        Debug.Log("Transfer transaction done nice");

        SampleOutputUtil.PrintResult("Transfer transaction created", "SygmaClient", "BuildTransferTransaction");
        var transactionHash = await Web3Accessor.Web3.TransactionExecutor.SendTransaction(transferTransaction);
        SampleOutputUtil.PrintResult("Transaction hash: " + transactionHash.Hash, "SygmaClient", "SendTransaction");
    }
    
    public static async Task<(object[], TransactionReceipt)> MintErc1155(Web3 web3, string abi, string contractAddress, BigInteger id, BigInteger amount)
    {
        byte[] dataObject = { };
        const string method = EthMethod.Mint;
        var destination = await web3.Signer.GetAddress();
        var contract = web3.ContractBuilder.Build(abi, contractAddress);
        var response = await contract.SendWithReceipt(method, new object[]
        {
            destination,
            id,
            amount,
            dataObject
        });
        return response;
    }}