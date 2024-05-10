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
    [SerializeField] private string name = "ChainSafe";
    [SerializeField] private string description = "An NFT description";

    #endregion
    
    #region Methods
    
    /// <summary>
    /// Uploads an image selected by the user including metadata to IPFS
    /// </summary>
    public void IPFSUpload()
    {
        var uploadRequest = new IPFSUploadRequestModel
        {
            ApiKey = apiKey,
            BucketId = bucketId,
            FileNameImage = fileNameImage,
            FileNameMetaData = fileNameMetaData,
            Name = name,
            Description = description
        };
        IPFS.UploadImageFromFile(uploadRequest);
    }

    #endregion
}
