using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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

public class EvmCalls : MonoBehaviour
{
    #region Fields
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
    [SerializeField] private string[] stringArraySend =
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
    public async void ContractCall()
    {
        object[] args =
        {
            Web3Accessor.Web3.Signer.PublicAddress
        };
        var response = await Evm.ContractCall(Web3Accessor.Web3, methodCall, ABI.ArrayTotal, ChainSafeContracts.ArrayTotal, args);
        Debug.Log(response);
        var output = SampleOutputUtil.BuildOutputValue(response);
        SampleOutputUtil.PrintResult(output, nameof(Evm), nameof(Evm.ContractCall));
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
        var response = await Evm.ContractSend(Web3Accessor.Web3, methodSend, ABI.ArrayTotal, ChainSafeContracts.ArrayTotal, args);
        var output = SampleOutputUtil.BuildOutputValue(response);
        SampleOutputUtil.PrintResult(output, nameof(Evm), nameof(Evm.ContractSend));
    }

    /// <summary>
    /// Gets array values from a contract
    /// </summary>
    public async void GetArray()
    {
        var response = await Evm.GetArray<string>(Web3Accessor.Web3, ChainSafeContracts.ArrayTotal, ABI.ArrayTotal, methodArrayGet);
        var responseString = string.Join(",\n", response.Select((list, i) => $"#{i} {string.Join((string)", ", (IEnumerable<string>)list)}"));
        SampleOutputUtil.PrintResult(responseString, nameof(Evm), nameof(Evm.GetArray));
    }

    /// <summary>
    /// Sends array values to a contract
    /// </summary>
    public async void SendArray()
    {
        var response = await Evm.SendArray(Web3Accessor.Web3, methodArraySend, ABI.ArrayTotal, ChainSafeContracts.ArrayTotal, stringArraySend);
        var output = SampleOutputUtil.BuildOutputValue(response);
        SampleOutputUtil.PrintResult(output, nameof(Evm), nameof(Evm.SendArray));
    }

    /// <summary>
    /// Gets the current block number
    /// </summary>
    public async void GetBlockNumber()
    {
        var blockNumber = await Evm.GetBlockNumber(Web3Accessor.Web3);
        SampleOutputUtil.PrintResult(blockNumber.ToString(), nameof(Evm), nameof(Evm.GetBlockNumber));
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
        var gasLimit = await Evm.GetGasLimit(Web3Accessor.Web3, ABI.ArrayTotal, ChainSafeContracts.ArrayTotal, methodSend, args);
        SampleOutputUtil.PrintResult(gasLimit.ToString(), nameof(Evm), nameof(Evm.GetGasLimit));
    }

    /// <summary>
    /// Gets the current gas price
    /// </summary>
    public async void GetGasPrice()
    {
        var gasPrice = await Evm.GetGasPrice(Web3Accessor.Web3);
        SampleOutputUtil.PrintResult(gasPrice.ToString(), nameof(Evm), nameof(Evm.GetGasPrice));
    }

    /// <summary>
    /// Gets an accounts nonce
    /// </summary>
    public async void GetNonce()
    {
        var nonce = await Evm.GetNonce(Web3Accessor.Web3);
        SampleOutputUtil.PrintResult(nonce.ToString(), nameof(Evm), nameof(Evm.GetNonce));
    }

    /// <summary>
    /// Gets a specific transaction's status
    /// </summary>
    public async void GetTransactionStatus()
    {
        var receipt = await Evm.GetTransactionStatus(Web3Accessor.Web3);
        var output = $"Confirmations: {receipt.Confirmations}," +
                     $" Block Number: {receipt.BlockNumber}," +
                     $" Status {receipt.Status}";

        SampleOutputUtil.PrintResult(output, nameof(Evm), nameof(Evm.GetTransactionStatus));
    }

    /// <summary>
    /// Uses a registered contract
    /// </summary>
    public async void RegisteredContract()
    {
        var balance = await Evm.UseRegisteredContract(Web3Accessor.Web3, registeredContractName, EthMethods.BalanceOf);
        SampleOutputUtil.PrintResult(balance.ToString(), nameof(Evm), nameof(Evm.UseRegisteredContract));
    }

    /// <summary>
    /// Sends a transaction
    /// </summary>
    public async void SendTransaction()
    {
        var transactionHash = await Evm.SendTransaction(Web3Accessor.Web3, toAddress);
        SampleOutputUtil.PrintResult(transactionHash, nameof(Evm), nameof(Evm.SendTransaction));
    }

    /// <summary>
    /// Encrypts a message with SHA3
    /// </summary>
    public void Sha3()
    {
        var hash = Evm.Sha3(messageSha);
        SampleOutputUtil.PrintResult(hash, nameof(Evm), nameof(Evm.Sha3));
    }

    /// <summary>
    /// Signs a message, the result is specific to each user
    /// </summary>
    public async void SignMessage()
    {
        var signedMessage = await Evm.SignMessage(Web3Accessor.Web3, messageSign);
        SampleOutputUtil.PrintResult(signedMessage, nameof(Evm), nameof(Evm.SignMessage));
    }

    /// <summary>
    /// Signs a message and verify account ownership
    /// </summary>
    public async void SignVerify()
    {
        var signatureVerified = await Evm.SignVerify(Web3Accessor.Web3, messageSignVerify);
        var output = signatureVerified ? "Verified" : "Failed to verify";
        SampleOutputUtil.PrintResult(output, nameof(Evm), nameof(Evm.SignVerify));
    }

    /// <summary>
    /// Gets events data from a transaction
    /// </summary>
    public async void EventTxData()
    {
        // Contract write
        var amount = 1;
        object[] args =
        {
            amount
        };
        var contract = Web3Accessor.Web3.ContractBuilder.Build(ABI.ArrayTotal, eventContract);
        var data = await contract.SendWithReceipt("addTotal", args);
        // Quick pause to deal with chain congestion
        await new WaitForSeconds(2);
        // Event data from receipt
        var logs = data.receipt.Logs.Select(jToken => JsonConvert.DeserializeObject<FilterLog>(jToken.ToString()));
        var eventAbi = EventExtensions.GetEventABI<AmountIncreasedEvent>();
        var eventLogs = logs
            .Select(log => eventAbi.DecodeEvent<AmountIncreasedEvent>(log))
            .Where(l => l != null);

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
    }

    /// <summary>
    /// Signs a transaction via ECDSA
    /// </summary>
    public void EcdsaSignTransaction()
    {
        var result = Evm.EcdsaSignTransaction(ecdsaKey, transactionHash, chainId);
        SampleOutputUtil.PrintResult(result, nameof(Evm), nameof(Evm.EcdsaSignTransaction));
    }

    /// <summary>
    /// Signs a message via ECDSA
    /// </summary>
    public void EcdsaSignMessage()
    {
        var result = Evm.EcdsaSignMessage(ecdsaKey, ecdsaMessage);
        SampleOutputUtil.PrintResult(result, nameof(Evm), nameof(Evm.EcdsaSignMessage));
    }

    /// <summary>
    /// Gets an addres via ECDSA key
    /// </summary>
    public void EcdsaGetAddress()
    {
        var result = Evm.EcdsaGetAddress(ecdsaKey);
        SampleOutputUtil.PrintResult(result, nameof(Evm), nameof(Evm.EcdsaGetAddress));
    }

    /// <summary>
    /// Makes multiple calls
    /// </summary>
    public async void MultiCall()
    {
        var erc20Contract = Web3Accessor.Web3.ContractBuilder.Build(ABI.Erc20, ChainSafeContracts.Erc20);
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

        var multicallResultResponse = await Web3Accessor.Web3.MultiCall().MultiCallAsync(calls);

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
    }
}
