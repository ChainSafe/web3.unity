using System.Threading.Tasks;
using UnityEngine;
using Web3Unity.Scripts.Prefabs;

/* This prefab script should be copied & placed on the root of an object.
Change the class name, variables and add any additional changes at the end of the execute function.
The initialize function should be called by a method of your choosing */

/// <summary>
/// Uploads to IPFS
/// </summary>
public class IpfsUpload : MonoBehaviour
{
    // Variables
    private string apiKey = "YOUR_CHAINSAFE_STORE_API_KEY";
    private string data = "YOUR_DATA";
    private string bucketId = "BUCKET_ID";
    private string path = "/PATH";
    private string filename = "FILENAME.EXT";
    private IpfsSample logic;

    /// <summary>
    /// Starts the task, you can put this in the start function or call it from a button/event
    /// </summary>
    public async void InitializeTask()
    {
        // Sets the sample behaviour & executes
        logic = new IpfsSample();
        await ExecuteTask();
    }

    /// <summary>
    /// Executes the prefab task and sends the result to the console, you can also save this into a variable for later use
    /// </summary>
    private async Task ExecuteTask()
    {
        var cid = await logic.Upload(new IpfsUploadRequest
        {
            ApiKey = apiKey,
            Data = data,
            BucketId = bucketId,
            Path = path,
            Filename = filename
        });
        SampleOutputUtil.PrintResult(cid, nameof(IpfsSample), nameof(IpfsSample.Upload));
    }
}