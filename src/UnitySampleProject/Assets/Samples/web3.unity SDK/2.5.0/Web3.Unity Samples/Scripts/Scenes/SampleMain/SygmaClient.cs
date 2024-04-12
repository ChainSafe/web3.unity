using System;
using System.Numerics;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Transactions;
using ChainSafe.Gaming.SygmaClient;
using ChainSafe.Gaming.SygmaClient.Dto;
using ChainSafe.Gaming.SygmaClient.Types;
using ChainSafe.Gaming.UnityPackage;
using ChainSafe.Gaming.Web3;
using Nethereum.Hex.HexTypes;
using Scripts.EVM.Token;
using UnityEngine;
using UnityEngine.UI;
using Environment = ChainSafe.Gaming.SygmaClient.Types.Environment;

[Serializable]
public class SygmaConfig
{
    [field:SerializeField] public ResourceType ResourceType { get; private set; } = ResourceType.Erc1155;
    [field: SerializeField] public string TokenId { get; private set; } = "1";
    [field: SerializeField] public Environment SygmaEnvironment { get; private set; } = Environment.Testnet;
    [field: SerializeField] public uint DestinationChainId { get; private set; } = 338;
    [field: SerializeField] public uint Amount { get; private set; } = 1;
}

public class SygmaClient : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private SygmaConfig sygmaConfig;
    

    private ISygmaClient _sygmaClient;
    private bool _isSygmaInitialized;
    private Web3 _web3;

    private void Awake()
    {
        button.onClick.AddListener(CallSygma);
        _web3 = Web3Accessor.Web3;
        _sygmaClient = Web3Accessor.Web3.SygmaClient();
        _isSygmaInitialized = _sygmaClient.Initialize(sygmaConfig.SygmaEnvironment);

        if (!_isSygmaInitialized)
        {
            throw new Web3Exception("Sygma failed to initialize");
        }
    }

    private async void CallSygma()
    {
        if (!_isSygmaInitialized)
        {
            Debug.LogError("Can't call Sygma if it's not initialized");
            return;
        }
        var address = await _web3.Signer.GetAddress();
        
        Debug.Log("Starting sygma transfer");
        var transactionResponse = await _sygmaClient.Transfer(new SygmaTransferParams()
        {
            DestinationChainId = sygmaConfig.DestinationChainId,
            DestinationAddress = address,
            SourceAddress = address,
            Amount = new HexBigInteger(sygmaConfig.Amount),
            ResourceType = sygmaConfig.ResourceType,
            TokenId = sygmaConfig.TokenId
        });
        
        
        SampleOutputUtil.PrintResult("Transaction hash: " + transactionResponse.Hash, "SygmaClient", "SendTransaction");
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