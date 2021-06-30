using System.Collections;
using System.Numerics;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

public class ERC20
{
    private static string abi = "[ { \"inputs\": [ { \"internalType\": \"string\", \"name\": \"name_\", \"type\": \"string\" }, { \"internalType\": \"string\", \"name\": \"symbol_\", \"type\": \"string\" } ], \"stateMutability\": \"nonpayable\", \"type\": \"constructor\" }, { \"anonymous\": false, \"inputs\": [ { \"indexed\": true, \"internalType\": \"address\", \"name\": \"owner\", \"type\": \"address\" }, { \"indexed\": true, \"internalType\": \"address\", \"name\": \"spender\", \"type\": \"address\" }, { \"indexed\": false, \"internalType\": \"uint256\", \"name\": \"value\", \"type\": \"uint256\" } ], \"name\": \"Approval\", \"type\": \"event\" }, { \"anonymous\": false, \"inputs\": [ { \"indexed\": true, \"internalType\": \"address\", \"name\": \"from\", \"type\": \"address\" }, { \"indexed\": true, \"internalType\": \"address\", \"name\": \"to\", \"type\": \"address\" }, { \"indexed\": false, \"internalType\": \"uint256\", \"name\": \"value\", \"type\": \"uint256\" } ], \"name\": \"Transfer\", \"type\": \"event\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"owner\", \"type\": \"address\" }, { \"internalType\": \"address\", \"name\": \"spender\", \"type\": \"address\" } ], \"name\": \"allowance\", \"outputs\": [ { \"internalType\": \"uint256\", \"name\": \"\", \"type\": \"uint256\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"spender\", \"type\": \"address\" }, { \"internalType\": \"uint256\", \"name\": \"amount\", \"type\": \"uint256\" } ], \"name\": \"approve\", \"outputs\": [ { \"internalType\": \"bool\", \"name\": \"\", \"type\": \"bool\" } ], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"account\", \"type\": \"address\" } ], \"name\": \"balanceOf\", \"outputs\": [ { \"internalType\": \"uint256\", \"name\": \"\", \"type\": \"uint256\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"decimals\", \"outputs\": [ { \"internalType\": \"uint8\", \"name\": \"\", \"type\": \"uint8\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"spender\", \"type\": \"address\" }, { \"internalType\": \"uint256\", \"name\": \"subtractedValue\", \"type\": \"uint256\" } ], \"name\": \"decreaseAllowance\", \"outputs\": [ { \"internalType\": \"bool\", \"name\": \"\", \"type\": \"bool\" } ], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"spender\", \"type\": \"address\" }, { \"internalType\": \"uint256\", \"name\": \"addedValue\", \"type\": \"uint256\" } ], \"name\": \"increaseAllowance\", \"outputs\": [ { \"internalType\": \"bool\", \"name\": \"\", \"type\": \"bool\" } ], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"name\", \"outputs\": [ { \"internalType\": \"string\", \"name\": \"\", \"type\": \"string\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"symbol\", \"outputs\": [ { \"internalType\": \"string\", \"name\": \"\", \"type\": \"string\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"totalSupply\", \"outputs\": [ { \"internalType\": \"uint256\", \"name\": \"\", \"type\": \"uint256\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"recipient\", \"type\": \"address\" }, { \"internalType\": \"uint256\", \"name\": \"amount\", \"type\": \"uint256\" } ], \"name\": \"transfer\", \"outputs\": [ { \"internalType\": \"bool\", \"name\": \"\", \"type\": \"bool\" } ], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"sender\", \"type\": \"address\" }, { \"internalType\": \"address\", \"name\": \"recipient\", \"type\": \"address\" }, { \"internalType\": \"uint256\", \"name\": \"amount\", \"type\": \"uint256\" } ], \"name\": \"transferFrom\", \"outputs\": [ { \"internalType\": \"bool\", \"name\": \"\", \"type\": \"bool\" } ], \"stateMutability\": \"nonpayable\", \"type\": \"function\" } ]";
    
    public static async Task<BigInteger> BalanceOf(string _chain, string _network, string _contract, string _account, string _rpc="")
    {
        string method = "balanceOf";
        string[] obj = { _account };
        string args = JsonConvert.SerializeObject(obj);
        string response = await EVM.Call(_chain, _network, _contract, abi, method, args, _rpc);
        try 
        {
            return BigInteger.Parse(response);
        } 
        catch 
        {
            Debug.LogError(response);
            throw;
        }
    }

    public static async Task<string> Name(string _chain, string _network, string _contract, string _rpc="")
    {
        string method = "name";
        string[] obj = {};
        string args = JsonConvert.SerializeObject(obj);
        string response = await EVM.Call(_chain, _network, _contract, abi, method, args, _rpc);
        return response; 
    }

    public static async Task<string> Symbol(string _chain, string _network, string _contract, string _rpc="")
    {
        string method = "symbol";
        string[] obj = {};
        string args = JsonConvert.SerializeObject(obj);
        string response = await EVM.Call(_chain, _network, _contract, abi, method, args, _rpc);
        return response; 
    }

    public static async Task<BigInteger> Decimals(string _chain, string _network, string _contract, string _rpc="")
    {
        string method = "decimals";
        string[] obj = {};
        string args = JsonConvert.SerializeObject(obj);
        string response = await EVM.Call(_chain, _network, _contract, abi, method, args, _rpc);
        try 
        {
            return BigInteger.Parse(response);
        } 
        catch 
        {
            Debug.LogError(response);
            throw;
        }
    }

    public static async Task<BigInteger> TotalSupply(string _chain, string _network, string _contract, string _rpc="")
    {
        string method = "totalSupply";
        string[] obj = {};
        string args = JsonConvert.SerializeObject(obj);
        string response = await EVM.Call(_chain, _network, _contract, abi, method, args, _rpc);
        try 
        {
            return BigInteger.Parse(response);
        } 
        catch 
        {
            Debug.LogError(response);
            throw;
        }
    }
}
