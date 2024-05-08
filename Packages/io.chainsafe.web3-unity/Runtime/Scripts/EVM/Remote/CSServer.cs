using System.Threading.Tasks;
using ChainSafe.Gaming.UnityPackage;
using UnityEngine;
using UnityEngine.Networking;

namespace Scripts.EVM.Remote
{
    public class CSServer
    {
        private static readonly string host = "https://api.gaming.chainsafe.io/v1/projects/";

        /// <summary>
        /// Unity web request helper function to retrieve data.
        /// </summary>
        /// <param name="_path"></param>
        /// <returns></returns>
        public static async Task<string> GetData(string _path)
        {
            using UnityWebRequest webRequest = UnityWebRequest.Get($"{host}{Web3Accessor.Web3.ProjectConfig.ProjectId}{_path}");
            await webRequest.SendWebRequest();
            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error: " + webRequest.error);
                return null;
            }
            var json = webRequest.downloadHandler.text;
            return json;
        }
    
        /// <summary>
        /// Prints json properties in the console on new lines.
        /// </summary>
        /// <param name="obj">The object to print out</param>
        public static void PrintObject(object obj)
        {
            var properties = obj.GetType().GetProperties();
            foreach (var property in properties)
            {
                var value = property.GetValue(obj);
                Debug.Log($"{property.Name}: {value}");
            }
        }
    }
}