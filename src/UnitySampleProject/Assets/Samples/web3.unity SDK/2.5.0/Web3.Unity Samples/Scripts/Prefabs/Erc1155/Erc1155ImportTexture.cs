using System.Threading.Tasks;
using ChainSafe.Gaming.UnityPackage;
using UnityEngine;
using UnityEngine.UI;
using Web3Unity.Scripts.Prefabs;

/* This prefab script should be copied & placed on the root of an object.
Change the class name, variables and add any additional changes at the end of the execute function.
The initialize function should be called by a method of your choosing */

/// <summary>
/// Fetches the texture of an ERC1155 NFT and displays it to a raw image
/// </summary>
public class Erc1155ImportTexture : MonoBehaviour
{
    // Variables
    public RawImage textureContainer;
    private string contractAddress = "0x0288B4F1389ED7b3d3f9C7B73d4408235c0CBbc6";
    private string tokenId = "0";
    private Erc1155Sample logic;
    private Texture nullTexture;

    /// <summary>
    /// Starts the task, you can put this in the start function or call it from a button/event
    /// </summary>
    public async void InitializeTask()
    {
        logic = new Erc1155Sample(Web3Accessor.Web3);
        nullTexture = textureContainer.texture;
        await ExecuteTask();
    }

    /// <summary>
    /// Executes the prefab task and sends the result to the console, you can also save this into a variable for later use
    /// </summary>
    private async Task ExecuteTask()
    {
        textureContainer.texture = nullTexture;
        textureContainer.texture = await logic.ImportNftTexture(contractAddress, tokenId);
        SampleOutputUtil.PrintResult("Texture loaded", nameof(Erc1155Sample), nameof(Erc1155Sample.ImportNftTexture));
    }
}