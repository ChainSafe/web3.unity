using System.Text;
using System.Threading.Tasks;
using ChainSafe.GamingSdk.Evm.Unity;
using ChainSafe.GamingWeb3.Environment;
using Newtonsoft.Json;
using UnityEngine.Assertions;
using UnityEngine.Networking;

namespace ChainSafe.GamingWeb3.Unity
{
    public class UnityHttpClient : IHttpClient
    {
        private readonly IMainThreadRunner _mainThreadRunner;

        public UnityHttpClient(IMainThreadRunner mainThreadRunner)
        {
            _mainThreadRunner = mainThreadRunner;
        }

        public ValueTask<string> GetRaw(string url)
        {
            return new ValueTask<string>(_mainThreadRunner.EnqueueTask(async () =>
            {
                using var request = UnityWebRequest.Get(url);
                request.downloadHandler = new DownloadHandlerBuffer();
                await request.SendWebRequest();

                Assert.AreNotEqual(request.result, UnityWebRequest.Result.InProgress);

                if (request.result != UnityWebRequest.Result.Success)
                {
                    throw new Web3Exception($"HTTP.Get responded with error: {request.error}");
                }

                var response = request.downloadHandler.text;
                return response;
            }));
        }

        public ValueTask<string> PostRaw(string url, string data, string contentType)
        {
            return new ValueTask<string>(_mainThreadRunner.EnqueueTask(async () =>
            {
                using var request = new UnityWebRequest(url, "POST");
                request.uploadHandler = new UploadHandlerRaw(new UTF8Encoding().GetBytes(data));
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", contentType);
                await request.SendWebRequest();

                Assert.AreNotEqual(request.result, UnityWebRequest.Result.InProgress);

                if (request.result != UnityWebRequest.Result.Success)
                {
                    throw new Web3Exception($"HTTP.Post responded with error: {request.error}");
                }

                var response = request.downloadHandler.text;
                return response;
            }));
        }

        public async ValueTask<TResponse> Get<TResponse>(string url)
        {
            var responseJson = await GetRaw(url);
            var response = JsonConvert.DeserializeObject<TResponse>(responseJson);
            return response;
        }

        public async ValueTask<TResponse> Post<TRequest, TResponse>(string url, TRequest data)
        {
            var requestJson = JsonConvert.SerializeObject(data);
            var responseJson = await PostRaw(url, requestJson, "application/json");
            var response = JsonConvert.DeserializeObject<TResponse>(responseJson);
            return response;
        }
    }
}