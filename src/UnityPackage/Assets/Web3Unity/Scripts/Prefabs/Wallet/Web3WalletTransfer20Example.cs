using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Web3Unity.Scripts.Library.ETHEREUEM.EIP;
using Web3Unity.Scripts.Library.Ethers.Contracts;
using Web3Unity.Scripts.Library.Web3Wallet;

public class Web3WalletTransfer20Example : MonoBehaviour
{
    async public void OnTransfer20()
    {
        // https://chainlist.org/
        string chainId = "5"; // goerli
        // contract to interact with 
        string contract = "0xc778417e063141139fce010982780140aa0cd5ab";
        // value in wei
        string value = "0";
        // abi in json format
        string abi = ABI.ERC_20;
        // smart contract method to call
        string method = ETH_METHOD.Transfer;
        // account to send erc20 to
        string toAccount = "0xdD4c825203f97984e7867F11eeCc813A036089D1";
        // amount of erc20 tokens to send
        string amount = "1000000000000000";
        // create data to interact with smart contract
        var contractData = new Contract(abi, contract);
        var data = contractData.Calldata(method, new object[]
        {
            toAccount,
            amount
        });
        // gas limit OPTIONAL
        string gasLimit = "";
        // gas price OPTIONAL
        string gasPrice = "";
        // send transaction
        string response = await Web3Wallet.SendTransaction(chainId, contract, value, data, gasLimit, gasPrice);
        print(response);
    }
}
