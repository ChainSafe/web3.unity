using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Web3Unity.Scripts.Library.ETHEREUEM.WebGL
{
    public class GameLogger
    {
        private const string loggingUrl = "https://game-api-stg.chainsafe.io/logging/logEvent";

        public static async Task<string> Log(string _chain, string _network, object _data)
        {
            using var webRequest = new UnityWebRequest(loggingUrl, "POST");
            webRequest.timeout = -1;
            var bodyRaw = System.Text.Encoding.UTF8.GetBytes($"chain={_chain}&network={_network}&gameData={_data}");
            webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
            await webRequest.SendWebRequest();

            return webRequest.result switch
            {
                UnityWebRequest.Result.ProtocolError => webRequest.error,
                _ => webRequest.downloadHandler.text
            };
        }
    }
}