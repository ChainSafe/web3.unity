using Newtonsoft.Json;
using Web3Unity.Scripts.Prefabs;
using Scripts.EVM.Token;
using UnityEngine;

/* This prefab script should be copied & placed on the root of an object in a scene.
Change the class name, variables and add any additional changes at the end of the function.
The scripts function should be called by a method of your choosing - button, function etc */

/// <summary>
/// Uploads to IPFS
/// </summary>
public class Ipfs : MonoBehaviour
{
    // Variables
    private string apiKey = "eyJhbGciOiJFUzI1NiIsInR5cCI6IkpXVCJ9.eyJpYXQiOjE3MTUyNjg2MzAsImNuZiI6eyJqa3UiOiIvY2VydHMiLCJraWQiOiI5aHE4bnlVUWdMb29ER2l6VnI5SEJtOFIxVEwxS0JKSFlNRUtTRXh4eGtLcCJ9LCJ0eXBlIjoiYXBpX3NlY3JldCIsImlkIjo5NDc2LCJ1dWlkIjoiMmEyYWY0MWYtMTQ4MC00YWE0LWE1MDYtMjJkNjA3OTUzNjU4IiwicGVybSI6eyJiaWxsaW5nIjoiKiIsInNlYXJjaCI6IioiLCJzdG9yYWdlIjoiKiIsInVzZXIiOiIqIn0sImFwaV9rZXkiOiJHV0pCVFBWWU5BUklaRVhQTVhPTCIsInNlcnZpY2UiOiJzdG9yYWdlIiwicHJvdmlkZXIiOiIifQ._E7ZHMdmDGPA1y9bssvaTSTP4NU7Q-OqlJ2pe2nVpl0pEAwWJiDVjqmg4d8Xp_rXFsFY0bZW20ZzKloQ-JX9Ww";
    private string bucketId = "011266de-050d-4b5c-9c52-23dba893f67e";
    private string fileNameMetaData = "MetaData.json";
    private string fileNameImage = "Logo.png";
    
    public async void UploadImage()
    {
        var imagePath = UnityEditor.EditorUtility.OpenFilePanel("Select Image", "", "png,jpg,jpeg,gif");
        if (string.IsNullOrEmpty(imagePath)) return;
        var www = await new WWW("file://" + imagePath);
        IPFSUploadImage(www.texture);
    }
    
    private async void IPFSUploadImage(Texture2D image)
    {
        var cid = await Evm.IPFSUpload(new IpfsUploadRequest
        {
            ApiKey = apiKey,
            BucketId = bucketId,
            Filename = fileNameImage,
            Image = image
        });
        IPFSUploadMetaData(cid);
    }
    
    private async void IPFSUploadMetaData(string imageCid)
    {
        var metaDataObj = new Metadata
        {
            name = "ChainSafe",
            image = imageCid
        };
        var metaData = JsonConvert.SerializeObject(metaDataObj);
        var cid = await Evm.IPFSUpload(new IpfsUploadRequest
        {
            ApiKey = apiKey,
            Data = metaData,
            BucketId = bucketId,
            Filename = fileNameMetaData
        });
        Debug.Log($"Metadata CID: https://ipfs.chainsafe.io/ipfs/{cid}");
    }
}
