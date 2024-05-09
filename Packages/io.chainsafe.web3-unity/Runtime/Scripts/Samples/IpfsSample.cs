using System;
using System.Threading.Tasks;
using ChainSafe.Gaming.UnityPackage.Model;
using Newtonsoft.Json;
using Scripts.EVM.Token;
using UnityEngine;
using Web3Unity.Scripts.Library.IPFS;

namespace Web3Unity.Scripts.Prefabs
{
    public class IpfsSample
    {
        public static async Task<string> Upload(IPFSUploadRequestModel request)
        {
            var rawData = System.Text.Encoding.UTF8.GetBytes(request.Data);
            var ipfs = new Ipfs(request.ApiKey);
            var cid = await ipfs.Upload(request.BucketId, request.Filename, rawData, "application/octet-stream");
            return cid;
        }
        
        public static async void UploadImageFromFile(IPFSUploadRequestModel request)
        {
            try
            {
                var imagePath = UnityEditor.EditorUtility.OpenFilePanel("Select Image", "", "png,jpg,jpeg,gif");
                if (string.IsNullOrEmpty(imagePath)) return;
                var www = await new WWW("file://" + imagePath);
                // Upload image
                var imageCid = await Evm.IPFSUpload(new IPFSUploadRequestModel
                {
                    ApiKey = request.ApiKey,
                    BucketId = request.BucketId,
                    Filename = request.FileNameImage,
                    ImageTexture = www.texture
                });
                // Upload metadata with image
                var metaDataObj = new IPFSUploadRequestModel
                {
                    Name = request.Name,
                    Description = request.Description,
                    Image = imageCid
                };
                var metaData = JsonConvert.SerializeObject(metaDataObj);
                var cid = await Evm.IPFSUpload(new IPFSUploadRequestModel
                {
                    ApiKey = request.ApiKey,
                    Data = metaData,
                    BucketId = request.BucketId,
                    Filename = request.FileNameMetaData
                });
                Debug.Log($"Metadata CID: https://ipfs.chainsafe.io/ipfs/{cid}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error uploading metadata: {e}");
                throw;
            }
        }
        
    }
}