using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IPFSUploadExample : MonoBehaviour
{
    private const string apiKey = "YOUR_CHAINSAFE_STORE_API_KEY";
    
    async void Start()
    {
        var data = System.Text.Encoding.UTF8.GetBytes("YOUR_DATA");
        
        var ipfs = new IPFS(apiKey);
        var cid  = await ipfs.Upload("BUCKET_ID", "/PATH", "FILENAME.ext", data, "application/octet-stream");
        print(cid);
    }
}
