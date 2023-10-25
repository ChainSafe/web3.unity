using System.Linq;
using System.Threading.Tasks;
using ChainSafe.Gaming.UnityPackage;
using UnityEngine;
using Web3Unity.Scripts.Prefabs;

/* This prefab script should be copied & placed on the root of an object.
Change the class name, variables and add any additional changes at the end of the execute function.
The initialize function should be called by a method of your choosing */

/// <summary>
/// Fetches all ERC1155 NFTs from an account
/// </summary>
public class AllErc1155 : MonoBehaviour
{
    // Variables
    private string chain = "ethereum";
    private string network = "goerli"; // mainnet goerli
    private string account = "0xfaecAE4464591F8f2025ba8ACF58087953E613b1";
    // Optional
    private string contract = "";
    private int take = 1000;
    private int skip = 0;
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
        var allNfts = await logic.All(chain, network, account, contract, take, skip);
        var output = string.Join(",\n", allNfts.Select(nft => $"{nft.TokenId} - {nft.Uri}"));
        SampleOutputUtil.PrintResult(output, nameof(Erc1155Sample), nameof(Erc1155Sample.All));
    }
}