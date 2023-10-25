using System.Threading.Tasks;
using ChainSafe.Gaming.UnityPackage;
using UnityEngine;

/* This prefab script should be copied & placed on the root of an object.
Change the class name, variables and add any additional changes at the end of the execute function.
The initialize function should be called by a method of your choosing */

/// <summary>
/// Fetches the native balance of an ERC20 token from an account
/// </summary>
public class Erc20NativeBalanceOf : MonoBehaviour
{
    // Variables
    private string account;
    private Erc20Sample logic;

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
        logic = new Erc20Sample(Web3Accessor.Web3);
        await ExecuteTask();
    }
    
    /// <summary>
    /// Executes the prefab task and sends the result to the console, you can also save this into a variable for later use
    /// </summary>
    private async Task ExecuteTask()
    {
        var result = await logic.NativeBalanceOf(account);
        SampleOutputUtil.PrintResult(result.ToString(), nameof(Erc20Sample), nameof(Erc20Sample.NativeBalanceOf));
    }
}