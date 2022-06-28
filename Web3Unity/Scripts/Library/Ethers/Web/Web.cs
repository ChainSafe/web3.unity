using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine.Networking;

namespace Web3Unity.Scripts.Library.Ethers.Web
{
    public class Web
    {
        public static async Task<T> FetchJson<T>(string connection, string json)
        {
            var req = new UnityWebRequest(connection, "POST");
            req.uploadHandler = new UploadHandlerRaw(new UTF8Encoding().GetBytes(json));
            req.downloadHandler = new DownloadHandlerBuffer();
            req.SetRequestHeader("Content-Type", "application/json");
            await req.SendWebRequest();

            if (req.result != UnityWebRequest.Result.Success)
            {
                req.Dispose();
                throw new Exception(req.error);
            }

            var reader = new JsonTextReader(new StringReader(req.downloadHandler.text));
            var serializer = JsonSerializer.Create();
            var message = serializer.Deserialize<T>(reader);

            req.Dispose();

            return message;
        }
    }
}