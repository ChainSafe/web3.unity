using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ChainSafe.Gaming.UnityPackage.Model;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace Web3Unity.Scripts.Library.IPFS
{
    public static class IPFS
    {
        #region IPFS
        
        private static readonly string host = "https://api.chainsafe.io";

        #endregion

        [System.Serializable]
        public class GetFileInfoResponse
        {
            [System.Serializable]
            public class Content
            {
                public string cid;
            }

            public Content content;
        }
        
        public static async void UploadImageFromFile(IPFSUploadRequestModel request)
        {
            try
            {
                var imagePath = UnityEditor.EditorUtility.OpenFilePanel("Select Image", "", "png,jpg,jpeg,gif");
                if (string.IsNullOrEmpty(imagePath)) return;
                var www = await new WWW("file://" + imagePath);
                // Upload image
                var imageCid = await ConvertCIDUpload(new IPFSUploadRequestModel
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
                var metaData = JsonConvert.SerializeObject(metaDataObj, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                var cid = await ConvertCIDUpload(new IPFSUploadRequestModel
                {
                    ApiKey = request.ApiKey,
                    Data = metaData,
                    BucketId = request.BucketId,
                    Filename = request.FileNameMetaData
                });
                Debug.Log($"Metadata uploaded to https://ipfs.chainsafe.io/ipfs/{cid}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error uploading metadata: {e}");
                throw;
            }
        }
        
        private static async Task<string> ConvertCIDUpload(IPFSUploadRequestModel request)
        {
            var data = request.ImageTexture != null ? request.ImageTexture.EncodeToPNG() : Encoding.UTF8.GetBytes(request.Data);
            var cid = await Upload(request.ApiKey, request.BucketId, request.Filename, data, "application/octet-stream");
            return cid;
        }

        private static async Task<string> Upload(string apiKey, string bucketId, string filename, byte[] content, string contentType)
        {
            var formUpload = new List<IMultipartFormSection>
            {
                new MultipartFormFileSection("file", content, filename, contentType)
            };

            using var requestUpload = UnityWebRequest.Post(host + "/api/v1/bucket/" + bucketId + "/upload", formUpload);
            requestUpload.SetRequestHeader("Authorization", "Bearer " + apiKey);
            await requestUpload.SendWebRequest();

            if (requestUpload.result != UnityWebRequest.Result.Success)
            {
                throw new WebException(requestUpload.error);
            }

            // var jsonFile ="{\"path\": \""+path+"/"+filename+"\", \"source\": \""+bucketId+"\"}";
            var jsonFile = "{\"path\": \"" + filename + "\", \"source\": \"" + bucketId + "\"}";

            using var requestFile = new UnityWebRequest(host + "/api/v1/bucket/" + bucketId + "/file", "POST");
            requestFile.SetRequestHeader("Authorization", "Bearer " + apiKey);
            requestFile.SetRequestHeader("Content-Type", "application/json");
            requestFile.uploadHandler = new UploadHandlerRaw(new System.Text.UTF8Encoding().GetBytes(jsonFile));
            requestFile.downloadHandler = new DownloadHandlerBuffer();
            await requestFile.SendWebRequest();

            if (requestFile.result != UnityWebRequest.Result.Success)
            {
                throw new WebException(requestFile.error);
            }

            var data = JsonUtility.FromJson<GetFileInfoResponse>(requestFile.downloadHandler.text);
            return data.content.cid;
        }
    }
}