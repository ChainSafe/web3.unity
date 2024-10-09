using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChainSafe.Gaming.UnityPackage;
using ChainSafe.Gaming.Web3;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace Scripts.EVM.Remote
{
    /// <summary>
    /// Manages api calls.
    /// </summary>
    public class CSServer
    {
        #region Fields

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
            using UnityWebRequest webRequest = UnityWebRequest.Get($"{host}{Web3Unity.Web3.ProjectConfig.ProjectId}{_path}");
            await webRequest.SendWebRequest();
            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error: Your project ID doesn't have a marketplace or the token ID doesn't exist, please go to dashboard and create items: " + webRequest.error);
                return default;
            }
            var json = webRequest.downloadHandler.text;
            var response = JsonConvert.DeserializeObject<T>(json);
            return response;
        }

        /// <summary>
        /// Unity web request helper function to retrieve data.
        /// </summary>
        /// <param name="_path">The path suffix to call</param>
        /// <returns>Server response</returns>
        public static async Task<T> GetDataWithToken<T>(string _path, string _bearerToken, int offset = 0, int pageSize = 100)
        {
            string url = $"{host}{Web3Unity.Web3.ProjectConfig.ProjectId}{_path}?offset={offset}&pageSize={pageSize}";
            using UnityWebRequest webRequest = UnityWebRequest.Get(url);
            webRequest.SetRequestHeader("Authorization", $"Bearer {_bearerToken}");
            webRequest.SetRequestHeader("Content-Type", "application/json");
            await webRequest.SendWebRequest();
            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error fetching data, please ensure the email you logged in with has content in the dashboard: " + webRequest.error);
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
            using UnityWebRequest request = UnityWebRequest.Delete($"{host}{Web3Unity.Web3.ProjectConfig.ProjectId}{_path}");
            request.SetRequestHeader("Authorization", $"Bearer {_bearerToken}");
            await request.SendWebRequest();
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Error deleting: {request.error}");
                throw new Exception($"Error: {request.error}");
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
            var url = $"{host}{Web3Unity.Web3.ProjectConfig.ProjectId}{_path}";
            if (_path == "/nft?hash=blake2b-208")
            {
                url = "https://api.chainsafe.io/api/v1/nft?hash=blake2b-208";
            }
            using (UnityWebRequest request = UnityWebRequest.Post($"{url}", _formData))
            {
                request.SetRequestHeader("Authorization", $"Bearer {_bearerToken}");
                request.SetRequestHeader("Accept", "application/json");
                await request.SendWebRequest();
                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError("Creation failed: " + request.downloadHandler.text);
                    throw new Exception($"Error: {request.error}");
                }
                return request.downloadHandler.text;
            }
        }

        #endregion

    }
}