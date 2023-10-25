using System.Threading.Tasks;
using ChainSafe.Gaming.UnityPackage;
using UnityEngine;
using Web3Unity.Scripts.Prefabs;

/* This prefab script should be copied & placed on the root of an object.
Change the class name, variables and add any additional changes at the end of the execute function.
The initialize function should be called by a method of your choosing */

/// <summary>
/// Transfer ERC1155 tokens to an account
/// </summary>
public class Erc1155Transfer : MonoBehaviour
{
    // Variables
    private string toAccount = "0xdD4c825203f97984e7867F11eeCc813A036089D1";
    private string contractAddress = "0xe793e17Ec93bEc809C5Ac6dd0d8b383446E65B78";
    private int tokenId = 101;
    private int amount = 1;
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
        var response = await logic.TransferErc1155(contractAddress, tokenId, amount, toAccount);
        var output = SampleOutputUtil.BuildOutputValue(response);
        SampleOutputUtil.PrintResult(output, nameof(UnsortedSample), nameof(UnsortedSample.TransferErc1155));
    }
}