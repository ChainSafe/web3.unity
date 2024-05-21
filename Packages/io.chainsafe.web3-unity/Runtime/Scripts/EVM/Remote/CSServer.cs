using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using ChainSafe.Gaming.UnityPackage;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace Scripts.EVM.Remote
{
    public class CSServer
    {
        #region Fields
        
        [DllImport("__Internal")]
        private static extern void UploadImage();
        public static event EventHandler<byte[]> ImageSelected;
        private static readonly string host = "https://api.gaming.chainsafe.io/v1/projects/";

        #endregion

        #region Methods

        /// <summary>
        /// Unity web request helper function to retrieve data.
        /// </summary>
        /// <param name="_path">The path suffix to call</param>
        /// <returns>Server response</returns>
        public static async Task<T> GetData<T>(string _path)
        {
            using UnityWebRequest webRequest = UnityWebRequest.Get($"{host}{Web3Accessor.Web3.ProjectConfig.ProjectId}{_path}");
            await webRequest.SendWebRequest();
            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error: Your project ID doesn't have a marketplace or the token ID doesn't exist, please go to dashboard and create items " + webRequest.error);
                return default;
            }
            var json = webRequest.downloadHandler.text;
            var response = JsonConvert.DeserializeObject<T>(json);
            return response;
        }
        
        /// <summary>
        /// Unity web request helper function to delete data entries from collections/marketplace.
        /// </summary>
        /// <param name="_bearerToken">Bearer token to access dashboard services</param>
        /// <param name="_path">The path suffix to call</param>
        /// <returns>Server response</returns>
        public static async Task<string> DeleteData(string _bearerToken, string _path)
        {
            using UnityWebRequest request = UnityWebRequest.Delete($"{host}{Web3Accessor.Web3.ProjectConfig.ProjectId}{_path}");
            request.SetRequestHeader("Authorization", $"Bearer {_bearerToken}");
            await request.SendWebRequest();
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Error deleting: {request.error}");
                return request.error;
            }
            return "Deleted successfully";
        }
        
        /// <summary>
        /// Unity web request helper function to create data entries with collections/marketplace.
        /// </summary>
        /// <param name="_bearerToken">Bearer token to access dashboard services</param>
        /// <param name="_path">The path suffix to call</param>
        /// <param name="_formData">Form data used in the call</param>
        /// <returns>Server response</returns>
        public static async Task<string> CreateData(string _bearerToken, string _path,
            List<IMultipartFormSection> _formData)
        {
            using (UnityWebRequest request = UnityWebRequest.Post($"{host}{Web3Accessor.Web3.ProjectConfig.ProjectId}{_path}", _formData))
            {
                request.SetRequestHeader("Authorization", $"Bearer {_bearerToken}");
                await request.SendWebRequest();
                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError("Creation failed: " + request.downloadHandler.text);
                    return request.error;
                }
                return request.downloadHandler.text;
            }
        }

        #region Utilities
        
        /// <summary>
        /// Uploads an image in webgl builds
        /// </summary>
        /// <returns>Image data</returns>
        public static async Task<byte[]> UploadImageWebGL()
        {
            var imageDataTask = new TaskCompletionSource<byte[]>();
            // Event handler to set the result when the image is selected
            void OnImageSelectedHandler(object sender, byte[] imageData)
            {
                imageDataTask.SetResult(imageData);
                // Unsubscribe from the event after handling it
                ImageSelected -= OnImageSelectedHandler;
            }
            ImageSelected += OnImageSelectedHandler;
            UploadImage();
            var imageData = await imageDataTask.Task;
            return imageData;
        }
        
        /// <summary>
        /// Invokes event to pass image data from js function
        /// </summary>
        /// <param name="base64Data">Image data</param>
        public static void OnImageSelected(string imageData)
        {
            try
            {
                // Remove metadata from url
                var base64String = imageData.Substring(imageData.IndexOf(",") + 1);
                // Convert data URL to byte array
                byte[] imageDataBytes = Convert.FromBase64String(base64String);
                // Invoke event to complete the upload tasks
                ImageSelected?.Invoke(null, imageDataBytes);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        #endregion

        #endregion

    }
}