using System.Threading.Tasks;
using ChainSafe.Gaming.UnityPackage;
using UnityEngine;
using Web3Unity.Scripts.Prefabs;

/* This prefab script should be copied & placed on the root of an object.
Change the class name, variables and add any additional changes at the end of the execute function.
The initialize function should be called by a method of your choosing */

/// <summary>
/// Fetches the owner of an ERC721 token id
/// </summary>
public class Erc721OwnerOf : MonoBehaviour
{
    // Variables
    private string contractAddress = "0x06dc21f89f01409e7ed0e4c80eae1430962ae52c";
    private string tokenId = "0x01559ae4021a565d5cc4740f1cefa95de8c1fb193949ecd32c337b03047da501";
    private Erc721Sample logic;

    /// <summary>
    /// Starts the task, you can put this in the start function or call it from a button/event
    /// </summary>
    public async void InitializeTask()
    {
        // Sets the sample behaviour & executes
        logic = new Erc721Sample(Web3Accessor.Web3);
        await ExecuteTask();
    }

    /// <summary>
    /// Executes the prefab task and sends the result to the console, you can also save this into a variable for later use
    /// </summary>
    private async Task ExecuteTask()
    {
        var balance = await logic.OwnerOf(contractAddress, tokenId);
        SampleOutputUtil.PrintResult(balance.ToString(), nameof(Erc721Sample), nameof(Erc721Sample.OwnerOf));
    }
}