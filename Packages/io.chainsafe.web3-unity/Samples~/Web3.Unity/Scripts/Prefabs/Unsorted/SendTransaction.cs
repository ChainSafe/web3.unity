using System.Threading.Tasks;
using ChainSafe.Gaming.UnityPackage;
using Nethereum.Hex.HexTypes;
using UnityEngine;
using Web3Unity.Scripts.Prefabs;

/* This prefab script should be copied & placed on the root of an object.
Change the class name, variables and add any additional changes at the end of the execute function.
The initialize function should be called by a method of your choosing */

/// <summary>
/// Send a transaction
/// </summary>
public class SendTransaction : MonoBehaviour
{
    // Variables
    private string to = "0xdD4c825203f97984e7867F11eeCc813A036089D1";
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
        var transactionHash = await logic.SendTransaction(to);
        SampleOutputUtil.PrintResult(transactionHash, nameof(UnsortedSample), nameof(UnsortedSample.SendTransaction));
    }
}