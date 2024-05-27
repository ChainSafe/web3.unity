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

        private static readonly string host = "https://api.gaming.chainsafe.io/v1/projects/";

        #endregion

        #region Methods

        /// <summary>
        /// Unity web request helper function to retrieve data.
        /// </summary>
        /// <param name="_path"></param>
        /// <returns></returns>
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

        #endregion

    }
}