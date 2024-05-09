using System.Collections.Generic;
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
    [SerializeField] private string apiKey = "FILLOUT";
    [SerializeField] private string bucketId = "FILLOUT";
    [SerializeField] private string fileNameImage = "Logo.png";
    [SerializeField] private string fileNameMetaData = "MetaData.json";
    [SerializeField] private string name = "ChainSafe";
    [SerializeField] private string description = "An NFT description";
    
    public async void UploadImageFromFile()
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
            Image = image,
        });
        IPFSUploadMetaData(cid);
    }
    
    private async void IPFSUploadMetaData(string imageCid)
    {
        var metaDataObj = new IpfsUploadRequest.Metadata
        {
            name = name,
            description = description,
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
