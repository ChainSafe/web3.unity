using System.Threading.Tasks;
using ChainSafe.Gaming.UnityPackage;
using UnityEngine;

/* This prefab script should be copied & placed on the root of an object.
Change the class name, variables and add any additional changes at the end of the execute function.
The initialize function should be called by a method of your choosing */

/// <summary>
/// Fetches the total supply of an ERC20 contract
/// </summary>
public class Erc20TotalSupply : MonoBehaviour
{
    // Variables
    private string contractAddress = "0x3E0C0447e47d49195fbE329265E330643eB42e6f";
    private Erc20Sample logic;
    
    /// <summary>
    /// Starts the task, you can put this in the start function or call it from a button/event
    /// </summary>
    public async void InitializeTask()
    {
        logic = new Erc20Sample(Web3Accessor.Web3);
        await ExecuteTask();
    }
    
    /// <summary>
    /// Executes the prefab task and sends the result to the console, you can also save this into a variable for later use
    /// </summary>
    private async Task ExecuteTask()
    {
        var result = await logic.Name(contractAddress);
        SampleOutputUtil.PrintResult(result.ToString(), nameof(Erc20Sample), nameof(Erc20Sample.TotalSupply));
    }
}
