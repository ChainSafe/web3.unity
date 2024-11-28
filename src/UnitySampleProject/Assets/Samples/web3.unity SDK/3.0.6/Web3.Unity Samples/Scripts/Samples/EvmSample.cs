using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using ChainSafe.Gaming;
using ChainSafe.Gaming.Evm.Contracts.BuiltIn;
using ChainSafe.Gaming.Evm.Contracts.Extensions;
using ChainSafe.Gaming.MultiCall;
using ChainSafe.Gaming.UnityPackage;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts.QueryHandlers.MultiCall;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.RPC.Eth.DTOs;
using Newtonsoft.Json;
using Scripts.EVM.Token;
using UnityEngine;

public class EvmSample : MonoBehaviour, ISample
{
    #region Fields

    [field: SerializeField] public string Title { get; private set; }

    [field: SerializeField, TextArea] public string Description { get; private set; }

    public Type[] DependentServiceTypes => Array.Empty<Type>();

    [Header("Change the fields below for testing purposes")]

    #region Contract Send

    [Header("Contract Send")]
    [SerializeField] private string methodSend = "addTotal";
    [SerializeField] private int increaseAmountSend = 1;

    #endregion

    #region Contract Call

    [Header("Contract Call")]
    [SerializeField] private string methodCall = "myTotal";

    #endregion

    #region Get Send Array

    [Header("Array Calls")]
    [SerializeField] private string methodArrayGet = "getStore";
    [SerializeField] private string methodArraySend = "setStore";
    [SerializeField]
    private string[] stringArraySend =
    {
        "0xFb3aECf08940785D4fB3Ad87cDC6e1Ceb20e9aac",
        "0x92d4040e4f3591e60644aaa483821d1bd87001e3"
    };

    #endregion

    #region Sign Verify Sha3

    [Header("Sign Verify SHA3 calls")]
    [SerializeField] private string messageSign = "The right man in the wrong place can make all the difference in the world.";
    [SerializeField] private string messageSignVerify = "A man chooses, a slave obeys.";
    [SerializeField] private string messageSha = "It’s dangerous to go alone, take this!";

    #endregion

    #region Send Transaction

    [Header("Send Transaction Call")]
    [SerializeField] private string toAddress = "0xdD4c825203f97984e7867F11eeCc813A036089D1";
    [SerializeField] private string value = "12300000000000000";

    #endregion

    #region Registered Contract

    [Header("Registered Contract Call")]
    [SerializeField] private string registeredContractName = "CsTestErc20";

    #endregion

    #region ECDSA

    [Header("ECDSA Calls")]
    [SerializeField] private string ecdsaKey = "0x78dae1a22c7507a4ed30c06172e7614eb168d3546c13856340771e63ad3c0081";
    [SerializeField] private string ecdsaMessage = "This is a test message";
    [SerializeField] private string transactionHash = "0x123456789";
    [SerializeField] private string chainId = "11155111";

    #endregion

    #region Multi Call

    [Header("MutliCall")]
    [SerializeField] private string Erc20Account = "0xd25b827D92b0fd656A1c829933e9b0b836d5C3e2";

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
    public async Task<string> ContractCall()
    {
        object[] args =
        {
            Web3Unity.Instance.PublicAddress
        };
        var response = await Web3Unity.Instance.ContractCall(methodCall, ABI.ArrayTotal, ChainSafeContracts.ArrayTotal, args);

        return BuildToString(response);
    }

    /// <summary>
    /// Sends values to a contract
    /// </summary>
    public async Task<string> ContractSend()
    {
        object[] args =
        {
            increaseAmountSend
        };
        var response = await Web3Unity.Instance.ContractSend(methodSend, ABI.ArrayTotal, ChainSafeContracts.ArrayTotal, args);

        return BuildToString(response);
    }

    /// <summary>
    /// Gets the current block number
    /// </summary>
    public async Task<string> GetBlockNumber()
    {
        var blockNumber = await Web3Unity.Instance.GetBlockNumber();

        return blockNumber.ToString();
    }

    /// <summary>
    /// Gets the gas limit for a specific function
    /// </summary>
    public async Task<string> GetGasLimit()
    {
        object[] args =
        {
            increaseAmountSend
        };

        var gasLimit = await Web3Unity.Instance.GetGasLimit(ABI.ArrayTotal, ChainSafeContracts.ArrayTotal, methodSend, args);

        return gasLimit.ToString();
    }

    /// <summary>
    /// Gets the current gas price
    /// </summary>
    public async Task<string> GetGasPrice()
    {
        var gasPrice = await Web3Unity.Instance.GetGasPrice();

        return gasPrice.ToString();
    }

    /// <summary>
    /// Sends a transaction
    /// </summary>
    public async Task<string> SendTransaction()
    {
        var hash = await Web3Unity.Instance.SendTransaction(toAddress, BigInteger.Parse(value));

        return hash;
    }

    /// <summary>
    /// Signs a message, the result is specific to each user
    /// </summary>
    public async Task<string> SignMessage()
    {
        var signHash = await Web3Unity.Instance.SignMessage(messageSign);

        return signHash;
    }

    /// <summary>
    /// Gets events data from a transaction
    /// </summary>
    public async Task<string> EventTxData()
    {
        // Contract write
        var amount = 1;
        object[] args =
        {
            amount
        };
        var contract = Web3Unity.Web3.ContractBuilder.Build(ABI.ArrayTotal, eventContract);
        var data = await contract.SendWithReceipt("addTotal", args);
        // Quick pause to deal with chain congestion
        await new WaitForSeconds(2);
        // Event data from receipt
        var logs = data.receipt.Logs.Select(jToken => JsonConvert.DeserializeObject<FilterLog>(jToken.ToString()));
        var eventAbi = EventExtensions.GetEventABI<AmountIncreasedEvent>();
        var eventLogs = logs
            .Select(log => eventAbi.DecodeEvent<AmountIncreasedEvent>(log))
            .Where(l => l != null).ToArray();

        if (!eventLogs.Any())
        {
            Debug.Log("No event data");
        }
        else
        {
            Debug.Log("Event data found");
            foreach (var eventLog in eventLogs)
            {
                var eventData = eventLog.Event;
                Debug.Log($"Wallet from event data: {eventData.wallet}");
                Debug.Log($"Amount from event data: {eventData.amount}");
            }
        }

        return $"{nameof(EventTxData)} executed.";
    }

    /// <summary>
    /// Makes multiple calls
    /// </summary>
    public async Task<string> MultiCall()
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
                CallData = erc20BalanceOfCalldata.HexToByteArray(),
            },
            new Call3Value()
            {
                Target = ChainSafeContracts.Erc20,
                AllowFailure = true,
                CallData = erc20TotalSupplyCalldata.HexToByteArray(),
            }
        };

        var multicallResultResponse = await Web3Unity.Web3.MultiCall().MultiCallAsync(calls);

        Debug.Log(multicallResultResponse);

        if (multicallResultResponse[0] != null && multicallResultResponse[0].Success)
        {
            var decodedBalanceOf = erc20Contract.Decode(EthMethods.BalanceOf, multicallResultResponse[0].ReturnData.ToHex());
            Debug.Log($"decodedBalanceOf {((BigInteger)decodedBalanceOf[0]).ToString()}");
        }

        if (multicallResultResponse[1] != null && multicallResultResponse[1].Success)
        {
            var decodedTotalSupply = erc20Contract.Decode(EthMethods.TotalSupply, multicallResultResponse[1].ReturnData.ToHex());
            Debug.Log($"decodedTotalSupply {((BigInteger)decodedTotalSupply[0]).ToString()}");
        }

        return $"{nameof(MultiCall)} executed.";
    }
    
    private static string BuildToString(IEnumerable<object> dynamicResponse)
    {
        return string.Join(",\n", dynamicResponse.Select(o => o.ToString()));
    }
}