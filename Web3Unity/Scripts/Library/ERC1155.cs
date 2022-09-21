using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

public class ERC1155
{
    private static string
        abi =
            "[ { \"inputs\": [ { \"internalType\": \"string\", \"name\": \"uri_\", \"type\": \"string\" } ], \"stateMutability\": \"nonpayable\", \"type\": \"constructor\" }, { \"anonymous\": false, \"inputs\": [ { \"indexed\": true, \"internalType\": \"address\", \"name\": \"account\", \"type\": \"address\" }, { \"indexed\": true, \"internalType\": \"address\", \"name\": \"operator\", \"type\": \"address\" }, { \"indexed\": false, \"internalType\": \"bool\", \"name\": \"approved\", \"type\": \"bool\" } ], \"name\": \"ApprovalForAll\", \"type\": \"event\" }, { \"anonymous\": false, \"inputs\": [ { \"indexed\": true, \"internalType\": \"address\", \"name\": \"operator\", \"type\": \"address\" }, { \"indexed\": true, \"internalType\": \"address\", \"name\": \"from\", \"type\": \"address\" }, { \"indexed\": true, \"internalType\": \"address\", \"name\": \"to\", \"type\": \"address\" }, { \"indexed\": false, \"internalType\": \"uint256[]\", \"name\": \"ids\", \"type\": \"uint256[]\" }, { \"indexed\": false, \"internalType\": \"uint256[]\", \"name\": \"values\", \"type\": \"uint256[]\" } ], \"name\": \"TransferBatch\", \"type\": \"event\" }, { \"anonymous\": false, \"inputs\": [ { \"indexed\": true, \"internalType\": \"address\", \"name\": \"operator\", \"type\": \"address\" }, { \"indexed\": true, \"internalType\": \"address\", \"name\": \"from\", \"type\": \"address\" }, { \"indexed\": true, \"internalType\": \"address\", \"name\": \"to\", \"type\": \"address\" }, { \"indexed\": false, \"internalType\": \"uint256\", \"name\": \"id\", \"type\": \"uint256\" }, { \"indexed\": false, \"internalType\": \"uint256\", \"name\": \"value\", \"type\": \"uint256\" } ], \"name\": \"TransferSingle\", \"type\": \"event\" }, { \"anonymous\": false, \"inputs\": [ { \"indexed\": false, \"internalType\": \"string\", \"name\": \"value\", \"type\": \"string\" }, { \"indexed\": true, \"internalType\": \"uint256\", \"name\": \"id\", \"type\": \"uint256\" } ], \"name\": \"URI\", \"type\": \"event\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"account\", \"type\": \"address\" }, { \"internalType\": \"uint256\", \"name\": \"id\", \"type\": \"uint256\" } ], \"name\": \"balanceOf\", \"outputs\": [ { \"internalType\": \"uint256\", \"name\": \"\", \"type\": \"uint256\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"address[]\", \"name\": \"accounts\", \"type\": \"address[]\" }, { \"internalType\": \"uint256[]\", \"name\": \"ids\", \"type\": \"uint256[]\" } ], \"name\": \"balanceOfBatch\", \"outputs\": [ { \"internalType\": \"uint256[]\", \"name\": \"\", \"type\": \"uint256[]\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"account\", \"type\": \"address\" }, { \"internalType\": \"address\", \"name\": \"operator\", \"type\": \"address\" } ], \"name\": \"isApprovedForAll\", \"outputs\": [ { \"internalType\": \"bool\", \"name\": \"\", \"type\": \"bool\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"_address\", \"type\": \"address\" }, { \"internalType\": \"uint256\", \"name\": \"_amount\", \"type\": \"uint256\" } ], \"name\": \"ownerMint\", \"outputs\": [], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"from\", \"type\": \"address\" }, { \"internalType\": \"address\", \"name\": \"to\", \"type\": \"address\" }, { \"internalType\": \"uint256[]\", \"name\": \"ids\", \"type\": \"uint256[]\" }, { \"internalType\": \"uint256[]\", \"name\": \"amounts\", \"type\": \"uint256[]\" }, { \"internalType\": \"bytes\", \"name\": \"data\", \"type\": \"bytes\" } ], \"name\": \"safeBatchTransferFrom\", \"outputs\": [], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"from\", \"type\": \"address\" }, { \"internalType\": \"address\", \"name\": \"to\", \"type\": \"address\" }, { \"internalType\": \"uint256\", \"name\": \"id\", \"type\": \"uint256\" }, { \"internalType\": \"uint256\", \"name\": \"amount\", \"type\": \"uint256\" }, { \"internalType\": \"bytes\", \"name\": \"data\", \"type\": \"bytes\" } ], \"name\": \"safeTransferFrom\", \"outputs\": [], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"operator\", \"type\": \"address\" }, { \"internalType\": \"bool\", \"name\": \"approved\", \"type\": \"bool\" } ], \"name\": \"setApprovalForAll\", \"outputs\": [], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"bytes4\", \"name\": \"interfaceId\", \"type\": \"bytes4\" } ], \"name\": \"supportsInterface\", \"outputs\": [ { \"internalType\": \"bool\", \"name\": \"\", \"type\": \"bool\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"uint256\", \"name\": \"\", \"type\": \"uint256\" } ], \"name\": \"uri\", \"outputs\": [ { \"internalType\": \"string\", \"name\": \"\", \"type\": \"string\" } ], \"stateMutability\": \"view\", \"type\": \"function\" } ]";

    public enum BroadcastMethod
    {
        WebGL,
        Web3,
        PrivateKey
    }

    public static async Task<BigInteger>
    BalanceOf(
        string _chain,
        string _network,
        string _contract,
        string _account,
        string _tokenId,
        string _rpc = ""
    )
    {
        string method = "balanceOf";
        string[] obj = { _account, _tokenId };
        string args = JsonConvert.SerializeObject(obj);
        string response =
            await EVM
                .Call(_chain, _network, _contract, abi, method, args, _rpc);
        try
        {
            return BigInteger.Parse(response);
        }
        catch
        {
            Debug.LogError (response);
            throw;
        }
    }

    public static async Task<List<BigInteger>>
    BalanceOfBatch(
        string _chain,
        string _network,
        string _contract,
        string[] _accounts,
        string[] _tokenIds,
        string _rpc = ""
    )
    {
        string method = "balanceOfBatch";
        string[][] obj = { _accounts, _tokenIds };
        string args = JsonConvert.SerializeObject(obj);
        string response =
            await EVM
                .Call(_chain, _network, _contract, abi, method, args, _rpc);
        try
        {
            string[] responses =
                JsonConvert.DeserializeObject<string[]>(response);
            List<BigInteger> balances = new List<BigInteger>();
            for (int i = 0; i < responses.Length; i++)
            {
                balances.Add(BigInteger.Parse(responses[i]));
            }
            return balances;
        }
        catch
        {
            Debug.LogError (response);
            throw;
        }
    }

    public static async Task<string>
    URI(
        string _chain,
        string _network,
        string _contract,
        string _tokenId,
        string _rpc = ""
    )
    {
        string method = "uri";
        string[] obj = { _tokenId };
        string args = JsonConvert.SerializeObject(obj);
        string response =
            await EVM
                .Call(_chain, _network, _contract, abi, method, args, _rpc);
        return response;
    }

    public static async Task<string>
    SetApprovalForAll(
        BroadcastMethod broadcastMethod,
        string _chain,
        string _network,
        string _contract,
        string _operator,
        bool _approved,
        string _privateKey = "",
        string _rpc = ""
    )
    {
        string method = "setApprovalForAll";
        string gasLimit = "";

        string account = PlayerPrefs.GetString("Account");

        // I found that without asking for gas price it is often being miscalculated
        string gasPrice = await EVM.GasPrice(_chain, _network);
        string chainId = await EVM.ChainId(_chain, _network, _rpc);

        string[] obj = { _operator, _approved.ToString() };
        string args = JsonConvert.SerializeObject(obj);

        // Due to multiple ways of sending transacion we need to add support for different options
        if (broadcastMethod == BroadcastMethod.WebGL)
        {
            string response =
                await Web3GL
                    .SendContract(method,
                    abi,
                    _contract,
                    args,
                    "0",
                    gasLimit,
                    gasPrice);
            return response;
        }

        if (broadcastMethod == BroadcastMethod.PrivateKey)
        {
            string data = await EVM.CreateContractData(abi, method, args);
            string transaction =
                await EVM
                    .CreateTransaction(_chain,
                    _network,
                    account,
                    _contract,
                    "0",
                    data,
                    gasPrice,
                    gasLimit,
                    _rpc);
            string signature =
                Web3PrivateKey
                    .SignTransaction(_privateKey, transaction, chainId);
            string response =
                await EVM
                    .BroadcastTransaction(_chain,
                    _network,
                    account,
                    _contract,
                    "0",
                    data,
                    signature,
                    gasPrice,
                    gasLimit,
                    _rpc);
            return response;
        }

        if (broadcastMethod == BroadcastMethod.Web3)
        {
            string data = await EVM.CreateContractData(abi, method, args);
            string response =
                await Web3Wallet
                    .SendTransaction(chainId,
                    _contract,
                    "0",
                    data,
                    gasLimit,
                    gasPrice);
            return response;
        }
        return "";
    }
}
