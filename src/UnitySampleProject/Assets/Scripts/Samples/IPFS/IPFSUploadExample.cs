using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Web3Unity.Scripts.Library.IPFS;

public class IPFSUploadExample : MonoBehaviour
{
    [SerializeField]
    string apiKey = "YOUR_CHAINSAFE_STORE_API_KEY";

    [SerializeField]
    string data = "YOUR_DATA";

    [SerializeField]
    string bucketId = "BUCKET_ID";

    [SerializeField]
    string path = "/PATH";

    [SerializeField]
    string filename = "FILENAME.EXT";

    async void Start()
    {
        var data = System.Text.Encoding.UTF8.GetBytes(this.data);
        var ipfs = new Ipfs(apiKey);
        var cid = await ipfs.Upload(bucketId, path, filename, data, "application/octet-stream");
        print(cid);
    }
}
