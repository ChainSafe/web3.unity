using System.Numerics;
using ChainSafe.Gaming.Evm.Contracts.BuiltIn;
using ChainSafe.Gaming.MultiCall;
using ChainSafe.Gaming.UnityPackage;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts.QueryHandlers.MultiCall;
using Nethereum.Hex.HexConvertors.Extensions;
using Scripts.EVM.Token;
using UnityEngine;

public class EvmCalls : MonoBehaviour
{
    #region Fields

    [Header("Change the fields below for testing purposes")]

    #region Contract Send

    [Header("Contract Send")]
    [SerializeField]
    private string methodSend = "addTotal";

    [SerializeField] private int increaseAmountSend = 1;

    #endregion

    #region Contract Call

    [Header("Contract Call")] [SerializeField]
    private string methodCall = "myTotal";

    #endregion

    #region Get Send Array

    [Header("Array Calls")] [SerializeField]
    private string methodArrayGet = "getStore";

    [SerializeField] private string methodArraySend = "setStore";

    [SerializeField] private string[] stringArraySend =
    {
        "0xFb3aECf08940785D4fB3Ad87cDC6e1Ceb20e9aac",
        "0x92d4040e4f3591e60644aaa483821d1bd87001e3"
    };

    #endregion

    #region Sign Verify Sha3

    [Header("Sign Verify SHA3 calls")] [SerializeField]
    private string messageSign = "The right man in the wrong place can make all the difference in the world.";

    [SerializeField] private string messageSignVerify = "A man chooses, a slave obeys.";
    [SerializeField] private string messageSha = "It’s dangerous to go alone, take this!";

    #endregion

    #region Send Transaction

    [Header("Send Transaction Call")] [SerializeField]
    private string toAddress = "0xdD4c825203f97984e7867F11eeCc813A036089D1";

    [SerializeField] private string value = "12300000000000000";

    #endregion

    #region Registered Contract

    [Header("Registered Contract Call")] [SerializeField]
    private string registeredContractName = "CsTestErc20";

    #endregion

    #region ECDSA

    [Header("ECDSA Calls")] [SerializeField]
    private string ecdsaKey = "0x78dae1a22c7507a4ed30c06172e7614eb168d3546c13856340771e63ad3c0081";

    [SerializeField] private string ecdsaMessage = "This is a test message";
    [SerializeField] private string transactionHash = "0x123456789";
    [SerializeField] private string chainId = "11155111";

    #endregion

    #region Multi Call

    [Header("MutliCall")] [SerializeField] private string Erc20Account = "0xd25b827D92b0fd656A1c829933e9b0b836d5C3e2";

    #endregion

    #region EventData

    /// <summary>
    /// Class for the event data that we're calling, this must match the solidity event i.e. event AmountIncreased(address indexed wallet, uint256 amount);
    /// </summary>
    [Event("AmountIncreased")]
    private class AmountIncreasedEvent : IEventDTO
    {
        [Parameter("address", "wallet", 1, true)]
        public string wallet { get; set; }

        [Parameter("uint256", "amount", 2, false)]
        public BigInteger amount { get; set; }
    }

    private string eventContract = "0x9832B82746a4316E9E3A5e6c7ea02451bdAC4546";

    #endregion

    #endregion

    /// <summary>
    /// Calls values from a contract
    /// </summary>
    public async void ContractCall()
    {
        object[] args =
        {
            Web3Unity.Web3.Signer.PublicAddress
        };
        var response =
            await Web3Unity.Instance.ContractCall(methodCall, ABI.ArrayTotal, ChainSafeContracts.ArrayTotal, args);
        Debug.Log(response);
        var output = SampleOutputUtil.BuildOutputValue(response);
        SampleOutputUtil.PrintResult(output, nameof(Web3Unity), nameof(Web3Unity.ContractCall));
    }

    /// <summary>
    /// Sends values to a contract
    /// </summary>
    public async void ContractSend()
    {
        object[] args =
        {
            increaseAmountSend
        };
        var response =
            await Web3Unity.Instance.ContractSend(methodSend, ABI.ArrayTotal, ChainSafeContracts.ArrayTotal, args);
        var output = SampleOutputUtil.BuildOutputValue(response);
        SampleOutputUtil.PrintResult(output, nameof(Web3Unity), nameof(Web3Unity.ContractSend));
    }

    /// <summary>
    /// Gets the current block number
    /// </summary>
    public async void GetBlockNumber()
    {
        var blockNumber = await Web3Unity.Instance.GetBlockNumber();
        SampleOutputUtil.PrintResult(blockNumber.ToString(), nameof(Web3Unity), nameof(Web3Unity.GetBlockNumber));
    }

    /// <summary>
    /// Gets the gas limit for a specific function
    /// </summary>
    public async void GetGasLimit()
    {
        object[] args =
        {
            increaseAmountSend
        };
        var gasLimit =
            await Web3Unity.Instance.GetGasLimit(ABI.ArrayTotal, ChainSafeContracts.ArrayTotal, methodSend, args);
        SampleOutputUtil.PrintResult(gasLimit.ToString(), nameof(Web3Unity), nameof(Web3Unity.GetGasLimit));
    }

    /// <summary>
    /// Gets the current gas price
    /// </summary>
    public async void GetGasPrice()
    {
        var gasPrice = await Web3Unity.Instance.GetGasPrice();
        SampleOutputUtil.PrintResult(gasPrice.ToString(), nameof(Web3Unity), nameof(Web3Unity.GetGasPrice));
    }

    /// <summary>
    /// Encrypts a message with SHA3
    /// </summary>
    public void GetMessageHash()
    {
        var hash = Web3Unity.Instance.GetMessageHash(messageSha);
        SampleOutputUtil.PrintResult(hash, nameof(Web3Unity), nameof(Web3Unity.GetMessageHash));
    }

    /// <summary>
    /// Signs a message, the result is specific to each user
    /// </summary>
    public async void SignMessage()
    {
        var signedMessage = await Web3Unity.Instance.SignMessage(messageSign);
        SampleOutputUtil.PrintResult(signedMessage, nameof(Web3Unity), nameof(Web3Unity.SignMessage));
    }


    /// <summary>
    /// Signs a message via ECDSA
    /// </summary>
    public void SignMessageWithPrivateKey()
    {
        var result = Web3Unity.Instance.SignMessageWithPrivateKey(ecdsaKey, ecdsaMessage);
        SampleOutputUtil.PrintResult(result, nameof(Web3Unity), nameof(Web3Unity.SignMessageWithPrivateKey));
    }
    
    public void GetPublicAddressFromPrivateKey()
    {
        var result = Web3Unity.Instance.GetPublicAddressFromPrivateKey(ecdsaKey);
        SampleOutputUtil.PrintResult(result, nameof(Web3Unity), nameof(Web3Unity.GetPublicAddressFromPrivateKey));
    }

    /// <summary>
    /// Makes multiple calls
    /// </summary>
    public async void MultiCall()
    {
        var erc20Contract = Web3Unity.Web3.ContractBuilder.Build(ABI.Erc20, ChainSafeContracts.Erc20);
        var erc20BalanceOfCalldata = erc20Contract.Calldata(EthMethods.BalanceOf, new object[]
        {
            Erc20Account
        });

        var erc20TotalSupplyCalldata = erc20Contract.Calldata(EthMethods.TotalSupply, new object[]
        {
        });

        var calls = new[]
        {
            new Call3Value()
            {
                Target = ChainSafeContracts.Erc20,
                AllowFailure = true,
                CallData = erc20BalanceOfCalldata.HexToByteArray()
            },
            new Call3Value()
            {
                Target = ChainSafeContracts.Erc20,
                AllowFailure = true,
                CallData = erc20TotalSupplyCalldata.HexToByteArray()
            }
        };

        var multicallResultResponse = await Web3Unity.Web3.MultiCall().MultiCallAsync(calls);

        Debug.Log(multicallResultResponse);

        if (multicallResultResponse[0] != null && multicallResultResponse[0].Success)
        {
            var decodedBalanceOf =
                erc20Contract.Decode(EthMethods.BalanceOf, multicallResultResponse[0].ReturnData.ToHex());
            Debug.Log($"decodedBalanceOf {((BigInteger)decodedBalanceOf[0]).ToString()}");
        }

        if (multicallResultResponse[1] != null && multicallResultResponse[1].Success)
        {
            var decodedTotalSupply =
                erc20Contract.Decode(EthMethods.TotalSupply, multicallResultResponse[1].ReturnData.ToHex());
            Debug.Log($"decodedTotalSupply {((BigInteger)decodedTotalSupply[0]).ToString()}");
        }
    }
}