using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;
using Web3Unity.Scripts.Library.Ethers.Contracts;
using Web3Unity.Scripts.Library.Ethers.Providers;

#if UNITY_WEBGL
public class WebGLGetArray : MonoBehaviour
{
    // contract to interact with 
    string contractAddress = "0x5244d0453A727EDa96299384370359f4A2B5b20a";
    // abi in json format
    string abi =
        "[{\"inputs\":[{\"internalType\":\"address[]\",\"name\":\"_addresses\",\"type\":\"address[]\"}],\"name\":\"setStore\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"name\":\"bought\",\"outputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"getStore\",\"outputs\":[{\"internalType\":\"address[]\",\"name\":\"\",\"type\":\"address[]\"}],\"stateMutability\":\"view\",\"type\":\"function\"}]";
    // smart contract method to call
    string method = "getStore";
    // Address TextField
    public Text playerAddresses;
    public async void GetArrayDataWeb()
    {
        var contract = new Contract(abi, contractAddress, RPC.GetInstance.Provider());
        Debug.Log("Gas Price: " + await RPC.GetInstance.Provider().GetGasPrice());
        var gasPrice = await RPC.GetInstance.Provider().GetGasPrice();
        var gasValue = await contract.EstimateGas("getStore", new object[] { });
        Debug.Log("Gas Value: " + gasValue.Value);
        var calldata = await contract.Call(method);
        string json = JsonConvert.SerializeObject(calldata[0], Formatting.Indented);
        string[] addresses = JsonConvert.DeserializeObject<string[]>(json);
        if (addresses != null) Debug.Log("Addresses: " + addresses[0]);
        if (addresses != null) playerAddresses.text = addresses[0];
    }
}
#endif
