using System.Threading.Tasks;
using ChainSafe.Gaming.UnityPackage;
using UnityEngine;
using Web3Unity.Scripts.Prefabs;

/* This prefab script should be copied & placed on the root of an object.
Change the class name, variables and add any additional changes at the end of the execute function.
The initialize function should be called by a method of your choosing */

/// <summary>
/// Fetches the URI from an ERC1155 NFT
/// </summary>
public class Erc1155Uri : MonoBehaviour
{
    // Variables
    private string contractAddress = "0x2c1867BC3026178A47a677513746DCc6822A137A";
    private string tokenId = "0x01559ae4021aee70424836ca173b6a4e647287d15cee8ac42d8c2d8d128927e5";
    private Erc1155Sample logic;

    /// <summary>
    /// Starts the task, you can put this in the start function or call it from a button/event
    /// </summary>
    public async void InitializeTask()
    {
        // Sets the sample behaviour & executes
        logic = new Erc1155Sample(Web3Accessor.Web3);
        await ExecuteTask();
    }

    /// <summary>
    /// Executes the prefab task and sends the result to the console, you can also save this into a variable for later use
    /// </summary>
    private async Task ExecuteTask()
    {
        var uri = await logic.Uri(contractAddress, tokenId);
        SampleOutputUtil.PrintResult(uri, nameof(Erc1155Sample), nameof(Erc1155Sample.Uri));
    }
}