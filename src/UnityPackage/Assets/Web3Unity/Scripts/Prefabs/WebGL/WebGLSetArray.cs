using UnityEngine;
using Web3Unity.Scripts.Library.Ethers.Contracts;

#if UNITY_WEBGL
public class WebGLSetArray : MonoBehaviour
{
    public async void SetArrayObject()
    {
        // contract to interact with 
        var contractAddress = "0x5244d0453A727EDa96299384370359f4A2B5b20a";
        // value in wei
        var value = "0";
        // abi in json format
        var abi =
            "[{\"inputs\":[{\"internalType\":\"address[]\",\"name\":\"_addresses\",\"type\":\"address[]\"}],\"name\":\"setStore\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"name\":\"bought\",\"outputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"getStore\",\"outputs\":[{\"internalType\":\"address[]\",\"name\":\"\",\"type\":\"address[]\"}],\"stateMutability\":\"view\",\"type\":\"function\"}]";
        // smart contract method to call
        var method = "setStore";
        string[] stringArray =
            {"0xFb3aECf08940785D4fB3Ad87cDC6e1Ceb20e9aac", "0x92d4040e4f3591e60644aaa483821d1bd87001e3"};
        var contract = new Contract(abi, contractAddress);
        // gas limit OPTIONAL
        var gasLimit = "";
        // gas price OPTIONAL
        var gasPrice = "";
        var calldata = contract.Calldata(method, new object[]
        {
            stringArray
        });
        Debug.Log("Contract Data: " + calldata[0]);
        // send transaction
        var response = await Web3GL.SendTransactionData(contractAddress, value, gasLimit, gasPrice, calldata);
        print(response);
    }
}
#endif
