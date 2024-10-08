using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChainSafe.Gaming;
using ChainSafe.Gaming.UnityPackage.Model;
using UnityEngine;
using ChainSafe.Gaming.Marketplace;

public class IpfsSample : MonoBehaviour, ISample
{
    #region Fields

    [field: SerializeField] public string Title { get; private set; }

    [field: SerializeField, TextArea] public string Description { get; private set; }

    public Type[] DependentServiceTypes => Array.Empty<Type>();

    [Header("IPFS VALUES")]
    [SerializeField] private string apiSecretKey = "Fill In Your API Secret Key From Storage";
    [SerializeField] private string bucketId = "Fill In Your Bucket ID From Storage";
    [SerializeField] private string fileNameImage = "Logo.png";
    [SerializeField] private string fileNameMetaData = "MetaData.json";
    [SerializeField] private string nftName = "Name of the NFT";
    [SerializeField] private string description = "An NFT description";
    [SerializeField] private string externalUrl = "The URL that appears below your assets image";
    [SerializeField] private List<string> display_types = new List<string> { "Stamina", "Boost Number" };
    [SerializeField] private List<string> trait_types = new List<string> { "Health", "Thunder Power" };
    [SerializeField] private List<string> values = new List<string> { "5", "20" };
    [Header("Required for image only upload")]
    [SerializeField] private string imageCID = "Enter your image CID from storage or upload call";

    #endregion

    #region Methods

    /// <summary>
    /// Uploads an image selected by the user to IPFS
    /// </summary>
    public async Task<string> IPFSUploadImage()
    {
        var uploadRequest = new IPFSUploadRequestModel
        {
            ApiKey = apiSecretKey,
            BucketId = bucketId,
            FileNameImage = fileNameImage
        };
        var cid = await IPFS.UploadImage(uploadRequest);
        return $"Image uploaded to https://ipfs.chainsafe.io/ipfs/{cid}";
    }

    /// <summary>
    /// Uploads metadata to IPFS
    /// </summary>
    public async Task<string> IPFSUploadMetadata()
    {
        var uploadRequest = new IPFSUploadRequestModel
        {
            ApiKey = apiSecretKey,
            BucketId = bucketId,
            Image = imageCID,
            FileNameMetaData = fileNameMetaData,
            Name = nftName,
            Description = description,
            External_url = externalUrl,
            attributes = IPFS.CreateAttributesList(display_types, trait_types, values)
        };
        var cid = await IPFS.UploadMetaData(uploadRequest);
        return $"Metadata uploaded to https://ipfs.chainsafe.io/ipfs/{cid}";
    }

    /// <summary>
    /// Uploads an image selected by the user including metadata to IPFS
    /// </summary>
    public async Task<string> IPFSUploadImageAndMetadata()
    {
        var uploadRequest = new IPFSUploadRequestModel
        {
            ApiKey = apiSecretKey,
            BucketId = bucketId,
            FileNameImage = fileNameImage,
            FileNameMetaData = fileNameMetaData,
            Name = name,
            Description = description,
            External_url = externalUrl,
            attributes = IPFS.CreateAttributesList(display_types, trait_types, values)
        };
        var cid = await IPFS.UploadImageAndMetadata(uploadRequest);
        return $"Image & metadata uploaded to https://ipfs.chainsafe.io/ipfs/{cid}";
    }

    #endregion
}
