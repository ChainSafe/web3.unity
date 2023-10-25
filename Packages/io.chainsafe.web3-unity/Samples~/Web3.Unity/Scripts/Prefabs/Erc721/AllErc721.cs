using System.Linq;
using System.Threading.Tasks;
using ChainSafe.Gaming.UnityPackage;
using UnityEngine;
using Web3Unity.Scripts.Prefabs;

/* This prefab script should be copied & placed on the root of an object.
Change the class name, variables and add any additional changes at the end of the execute function.
The initialize function should be called by a method of your choosing */

/// <summary>
/// Fetches all ERC721 NFTs from an account
/// </summary>
public class AllErc721 : MonoBehaviour
{
    // Variables
    private string chain = "ethereum";
    private string network = "goerli";
    private string account = "0xfaecAE4464591F8f2025ba8ACF58087953E613b1";
    // Optional
    private string contract = "0x2c1867BC3026178A47a677513746DCc6822A137A";
    private int take = 500;
    private int skip = 0;
    private Erc721Sample logic;

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
        logic = new Erc721Sample(Web3Accessor.Web3);
        await ExecuteTask();
    }

    /// <summary>
    /// Executes the prefab task and sends the result to the console, you can also save this into a variable for later use
    /// </summary>
    private async Task ExecuteTask()
    {
        var allNfts = await logic.All(chain, network, account, contract, take, skip);
        var output = string.Join(",\n", allNfts.Select(nft => $"{nft.TokenId} - {nft.Uri}"));
        SampleOutputUtil.PrintResult(output, nameof(Erc721Sample), nameof(Erc721Sample.All));
    }
}