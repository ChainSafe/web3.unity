using System.Threading.Tasks;
using ChainSafe.Gaming.UnityPackage;
using UnityEngine;
using Web3Unity.Scripts.Prefabs;

/* This prefab script should be copied & placed on the root of an object.
Change the class name, variables and add any additional changes at the end of the execute function.
The initialize function should be called by a method of your choosing */

/// <summary>
/// Send an array to a contract
/// </summary>
public class SendArray : MonoBehaviour
{
    // Variables
    private string contractAddress = "0x5244d0453A727EDa96299384370359f4A2B5b20a";
    private string abi = "[{\"inputs\":[{\"internalType\":\"address[]\",\"name\":\"_addresses\",\"type\":\"address[]\"}],\"name\":\"setStore\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"name\":\"bought\",\"outputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"getStore\",\"outputs\":[{\"internalType\":\"address[]\",\"name\":\"\",\"type\":\"address[]\"}],\"stateMutability\":\"view\",\"type\":\"function\"}]";
    private string method = "setStore";
    private string[] stringArray =
    {
        "0xFb3aECf08940785D4fB3Ad87cDC6e1Ceb20e9aac",
        "0x92d4040e4f3591e60644aaa483821d1bd87001e3"
    };
    private UnsortedSample logic;

    /// <summary>
    /// Starts the task, you can put this in the start function or call it from a button/event
    /// </summary>
    public async void InitializeTask()
    {
        // Sets the sample behaviour & executes
        logic = new UnsortedSample(Web3Accessor.Web3);
        await ExecuteTask();
    }

    /// <summary>
    /// Executes the prefab task and sends the result to the console, you can also save this into a variable for later use
    /// </summary>
    private async Task ExecuteTask()
    {
        var response = await logic.SendArray(method, abi, contractAddress, stringArray);
        var output = SampleOutputUtil.BuildOutputValue(response);
        SampleOutputUtil.PrintResult(output, nameof(UnsortedSample), nameof(UnsortedSample.SendArray));
    }
}