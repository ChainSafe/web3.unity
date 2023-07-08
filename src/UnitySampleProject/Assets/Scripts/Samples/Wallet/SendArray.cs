using System;
using UnityEngine;

public class SendArray : MonoBehaviour
{
    public async void Start()
    {
        var contractAddress = "0x5244d0453A727EDa96299384370359f4A2B5b20a";
        var abi =
            "[{\"inputs\":[{\"internalType\":\"address[]\",\"name\":\"_addresses\",\"type\":\"address[]\"}],\"name\":\"setStore\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"name\":\"bought\",\"outputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"getStore\",\"outputs\":[{\"internalType\":\"address[]\",\"name\":\"\",\"type\":\"address[]\"}],\"stateMutability\":\"view\",\"type\":\"function\"}]";
        var method = "setStore";
        string[] stringArray =
            {"0xFb3aECf08940785D4fB3Ad87cDC6e1Ceb20e9aac", "0x92d4040e4f3591e60644aaa483821d1bd87001e3"};
        var contract = Web3Accessor.Web3.ContractBuilder.Build(abi, contractAddress);
        var response = await contract.Send(method, new object[]
        {
             stringArray
        });
        Debug.Log(response);
    }
}