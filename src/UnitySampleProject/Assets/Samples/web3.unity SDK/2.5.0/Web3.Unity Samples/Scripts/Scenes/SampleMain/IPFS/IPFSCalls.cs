using System.Collections.Generic;
using ChainSafe.Gaming.UnityPackage.Model;
using UnityEngine;
using Web3Unity.Scripts.Library.IPFS;

public class IPFSCalls : MonoBehaviour
{
    #region Fields

    [Header("IPFS VALUES")]
    [SerializeField] private string apiKey = "Fill In Your API Key From Storage";
    [SerializeField] private string bucketId = "Fill In Your Bucket ID From Storage";
    [SerializeField] private string fileNameImage = "Logo.png";
    [SerializeField] private string fileNameMetaData = "MetaData.json";
    [SerializeField] private string name = "Name of the NFT";
    [SerializeField] private string description = "An NFT description";
    [SerializeField] private string externalUrl = "The URL that appears below your assets image";
    [SerializeField] private List<string> display_types = new List<string> { "Stamina", "Boost Number" };
    [SerializeField] private List<string> trait_types = new List<string> { "Health", "Thunder Power" };
    [SerializeField] private List<string> values = new List<string> { "5", "20" };

    #endregion
    
    #region Methods
    
    /// <summary>
    /// Uploads an image selected by the user including metadata to IPFS
    /// </summary>
    public async void IPFSUpload()
    {
        var uploadRequest = new IPFSUploadRequestModel
        {
            ApiKey = apiKey,
            BucketId = bucketId,
            FileNameImage = fileNameImage,
            FileNameMetaData = fileNameMetaData,
            Name = name,
            Description = description,
            External_url = externalUrl,
            attributes = IPFS.CreateAttributesList(display_types, trait_types, values)
        };
        var cid = await IPFS.UploadImageFromFile(uploadRequest);
        Debug.Log($"Metadata uploaded to https://ipfs.chainsafe.io/ipfs/{cid}"); 
    }

    #endregion
}
