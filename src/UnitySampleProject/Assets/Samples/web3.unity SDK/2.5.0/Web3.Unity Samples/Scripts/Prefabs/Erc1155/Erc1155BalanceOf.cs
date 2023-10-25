using System.Threading.Tasks;
using ChainSafe.Gaming.UnityPackage;
using UnityEngine;
using Web3Unity.Scripts.Prefabs;

/* This prefab script should be copied & placed on the root of an object.
Change the class name, variables and add any additional changes at the end of the execute function.
The initialize function should be called by a method of your choosing */

/// <summary>
/// Fetches the balance of ERC1155 NFTs from an account
/// </summary>
public class Erc1155BalanceOf : MonoBehaviour
{
    // Variables
    private string contractAddress = "0x2c1867bc3026178a47a677513746dcc6822a137a";
    private string account = "0xd25b827D92b0fd656A1c829933e9b0b836d5C3e2";
    private string tokenId = "0x01559ae4021aee70424836ca173b6a4e647287d15cee8ac42d8c2d8d128927e5";
    private Erc1155Sample logic;

    /// <summary>
    /// Starts the task, you can put this in the start function or call it from a button/event
    /// </summary>
    public async void InitializeTask()
    {
        // Sets our account to be default if nothing has been entered
        if (string.IsNullOrEmpty(account))
        {
            account = PlayerPrefs.GetString("PlayerAccount");
        }

        // Sets the sample behaviour & executes
        logic = new Erc1155Sample(Web3Accessor.Web3);
        await ExecuteTask();
    }

    /// <summary>
    /// Executes the prefab task and sends the result to the console, you can also save this into a variable for later use
    /// </summary>
    private async Task ExecuteTask()
    {
        var balance = await logic.BalanceOf(contractAddress, account, tokenId);
        SampleOutputUtil.PrintResult(balance.ToString(), nameof(Erc1155Sample), nameof(Erc1155Sample.BalanceOf));
    }
}