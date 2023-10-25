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
public class Erc1155BalanceOfBatch : MonoBehaviour
{
    // Variables
    private string contractAddress = "0xdc4aff511e1b94677142a43df90f948f9ae181dd";
    private string[] accounts = { "0xd25b827D92b0fd656A1c829933e9b0b836d5C3e2", "0xE51995Cdb3b1c109E0e6E67ab5aB31CDdBB83E4a" };
    private string[] tokenIds = { "1", "2" };
    private Erc1155Sample logic;

    /// <summary>
    /// Starts the task, you can put this in the start function or call it from a button/event
    /// </summary>
    public async void InitializeTask()
    {
        // Sets our account to be default if nothing has been entered
        if (string.IsNullOrEmpty(accounts[0]))
        {
            accounts[0] = PlayerPrefs.GetString("PlayerAccount");
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
        var balances = await logic.BalanceOfBatch(contractAddress, accounts, tokenIds);
        var balancesString = string.Join(", ", balances);
        SampleOutputUtil.PrintResult(balancesString, nameof(Erc1155Sample), nameof(Erc1155Sample.BalanceOfBatch));
    }
}