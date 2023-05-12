using System.Diagnostics.Contracts;
using UnityEngine;
using Web3Unity.Scripts.Library.ETHEREUEM.EIP;
using Web3Unity.Scripts.Library.Ethers.Contracts;
using Web3Unity.Scripts.Library.Web3Wallet;
using Web3Unity.Scripts.Prefabs;
using Contract = Web3Unity.Scripts.Library.Ethers.Contracts.Contract;

public class Web3WalletTransfer721Example : MonoBehaviour
{
    public async void OnTransfer721()
    {
        // https://chainlist.org/
        var chainId = "5"; // goerli
        // contract to interact with 
        var contract = "0x31A61D3B956d9E95e0b9434BEf24bfEebB48b2c5";
        // value in wei
        var value = "0";
        // abi in json format
        var abi = ABI.ERC_721;
        // smart contract method to call
        var method = ETH_METHOD.SafeTransferFrom;
        // account to send erc721 to
        var toAccount = PlayerPrefs.GetString("Account");
        // token id to send
        var tokenId = "0";
        var contractData = new Contract(abi, contract);

        var data = contractData.Calldata(method, new object[]
        {
            toAccount,
            toAccount,
            tokenId
        });
        print(data);
        // gas limit OPTIONAL
        var gasLimit = "";
        // gas price OPTIONAL
        var gasPrice = "";
        // send transaction
        var response = await Web3Wallet.SendTransaction(chainId, contract, value, data, gasLimit, gasPrice);
        print(response);
    }
}