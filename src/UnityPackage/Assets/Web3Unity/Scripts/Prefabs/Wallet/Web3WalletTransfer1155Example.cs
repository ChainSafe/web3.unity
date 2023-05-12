using UnityEngine;
using Web3Unity.Scripts.Library.ETHEREUEM.EIP;
using Web3Unity.Scripts.Library.Ethers.Contracts;
using Web3Unity.Scripts.Library.Web3Wallet;

public class Web3WalletTransfer1155Example : MonoBehaviour
{
    public async void OnTransfer1155()
    {
        // https://chainlist.org/
        var chainId = "5"; // goerli
        // contract to interact with 
        var contract = "0xae283E79a5361CF1077bf2638a1A953c872AD973";
        // value in wei
        var value = "0";
        // abi in json format
        var abi = "";//ABI.ERC_1155;
        // smart contract method to call
        var method = "safeTransferFrom";
        // account to sent tokens to
        var toAccount = PlayerPrefs.GetString("Account");
        // token id to send
        var tokenId = 0;
        // amount of tokens to send
        var amount = 1;
        // bytes
        byte[] dataObject = { };
        // array of arguments for contract
        var contractData = new Contract(abi, contract);
        var data = contractData.Calldata(method, new object[]
        {
            PlayerPrefs.GetString("Account"),
            toAccount,
            tokenId,
            amount,
            dataObject
        });
        // gas limit OPTIONAL
        var gasLimit = "";
        // gas price OPTIONAL
        var gasPrice = "";
        // send transaction
        var response = await Web3Wallet.SendTransaction(chainId, contract, value, data, gasLimit, gasPrice);
        print(response);
    }
}