using System.Threading.Tasks;
using Prefabs.Web3AuthWallet.Interfaces;
using UnityEngine;
using UnityEngine.Networking;

namespace Prefabs.Web3AuthWallet.Utils
{
    public class HttpRequestHandler : IHttpRequestHandler
    {
        public async Task<T> PostRequest<T>(string url, WWWForm form)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Post(url, form))
            {
                await webRequest.SendWebRequest();
                return JsonUtility.FromJson<T>(System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data));
            }
        }
    }
}
