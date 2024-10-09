using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ChainSafe.Gaming.UnityPackage.Model;
using ChainSafe.Gaming.Web3;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace ChainSafe.Gaming.Marketplace
{
    public static class IPFS
    {
        #region Fields

        private static readonly string host = "https://api.chainsafe.io";

        #endregion

        #region Methods

        /// <summary>
        /// Creates a list of attributes
        /// </summary>
        /// <param name="display_types">List of display types</param>
        /// <param name="trait_types">List of trait types</param>
        /// <param name="values">List of trait values</param>
        /// <returns>List of IPFSUploadRequestModel.Attributes</returns>
        public static List<IPFSUploadRequestModel.Attributes> CreateAttributesList(List<string> display_types, List<string> trait_types, List<string> values)
        {
            // Create a list to store attributes
            var attributeList = new List<IPFSUploadRequestModel.Attributes>();
            for (int i = 0; i < display_types.Count; i++)
            {
                // Create a new instance of Attributes and set its properties
                var attribute = new IPFSUploadRequestModel.Attributes
                {
                    Display_types = new List<string> { display_types[i] },
                    Trait_types = new List<string> { trait_types[i] },
                    Values = new List<string> { values[i] }
                };
                // Add the attribute to the list
                attributeList.Add(attribute);
            }
            return attributeList;
        }

        /// <summary>
        /// Converts & uploads an image from file to IPFS
        /// </summary>
        /// <param name="request"></param>
        /// <returns>CID containing an image</returns>
        public static async Task<string> UploadImage(IPFSUploadRequestModel request)
        {
            try
            {
                // Upload metadata with image
                var imageData = await UploadPlatforms.GetImageData();
                var imageCid = await Upload(request.ApiKey, request.BucketId, request.FileNameImage, imageData, "application/octet-stream");
                return imageCid;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error uploading image: {e}");
                throw;
            }
        }

        /// <summary>
        /// Uploads a metadata file to IPFS
        /// </summary>
        /// <param name="request">IPFSUploadRequestModel</param>
        /// <returns>CID containing metadata</returns>
        public static async Task<string> UploadMetaData(IPFSUploadRequestModel request)
        {
            try
            {
                var metaDataObj = new IPFSUploadRequestModel
                {
                    Description = request.Description,
                    External_url = request.External_url,
                    Image = $"https://ipfs.chainsafe.io/ipfs/{request.Image}",
                    Name = request.Name,
                    attributes = request.attributes
                };
                // Serialize
                var metaData = JsonConvert.SerializeObject(metaDataObj, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                var data = Encoding.UTF8.GetBytes(metaData);
                var cid = await Upload(request.ApiKey, request.BucketId, request.FileNameMetaData, data, "application/octet-stream");
                return cid;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error uploading metadata: {e}");
                throw;
            }
        }

        /// <summary>
        /// Converts & uploads an image from file to IPFS
        /// </summary>
        /// <param name="request">IPFSUploadRequestModel</param>
        /// <returns>CID containing metadata & image</returns>
        public static async Task<string> UploadImageAndMetadata(IPFSUploadRequestModel request)
        {
            try
            {
                // Upload metadata with image
                var imageData = await UploadPlatforms.GetImageData();
                var imageCid = await Upload(request.ApiKey, request.BucketId, request.FileNameImage, imageData, "application/octet-stream");
                var metaDataObj = new IPFSUploadRequestModel
                {
                    Description = request.Description,
                    External_url = request.External_url,
                    Image = $"https://ipfs.chainsafe.io/ipfs/{imageCid}",
                    Name = request.Name,
                    attributes = request.attributes
                };
                // Serialize
                var metaData = JsonConvert.SerializeObject(metaDataObj, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                var data = Encoding.UTF8.GetBytes(metaData);
                var cid = await Upload(request.ApiKey, request.BucketId, request.FileNameMetaData, data, "application/octet-stream");
                return cid;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error uploading image & metadata: {e}");
                throw;
            }
        }

        /// <summary>
        /// Uploads a file to IPFS
        /// </summary>
        /// <param name="apiKey">Chainsafe API key</param>
        /// <param name="bucketId">Chainsafe bucket ID from the dashboard</param>
        /// <param name="filename">Name of the file being uploaded</param>
        /// <param name="content">The data content in bytes</param>
        /// <param name="contentType">The type of content being uploaded</param>
        /// <returns>The CID of the file uploaded</returns>
        /// <exception cref="WebException">Web request error</exception>
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

            var uploadResponse = JsonConvert.DeserializeObject<Path>(requestUpload.downloadHandler.text);
            if (uploadResponse.files_details[0].status == "failed")
            {
                throw new Web3Exception($"Upload Failed: {uploadResponse.files_details[0].message}");
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

        #endregion
    }
}