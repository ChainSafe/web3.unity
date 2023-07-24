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
        private readonly IMainThreadRunner mainThreadRunner;

        public UnityHttpClient(IMainThreadRunner mainThreadRunner)
        {
            this.mainThreadRunner = mainThreadRunner;
        }

        private static NetworkResponse<string> UnityWebRequestToNetworkResponse(UnityWebRequest request)
        {
            Assert.AreNotEqual(request.result, UnityWebRequest.Result.InProgress);

            if (request.result != UnityWebRequest.Result.Success)
            {
                throw new Web3Exception($"HTTP.Get responded with error: {request.error}");
            }

            return NetworkResponse<string>.Success(request.downloadHandler.text);
        }

        public ValueTask<NetworkResponse<string>> GetRaw(string url)
        {
            return new ValueTask<NetworkResponse<string>>(mainThreadRunner.EnqueueTask(async () =>
            {
                using var request = UnityWebRequest.Get(url);
                request.downloadHandler = new DownloadHandlerBuffer();
                await request.SendWebRequest();
                return UnityWebRequestToNetworkResponse(request);
            }));
        }

        public ValueTask<NetworkResponse<string>> PostRaw(string url, string data, string contentType)
        {
            return new ValueTask<NetworkResponse<string>>(mainThreadRunner.EnqueueTask(async () =>
            {
                using var request = new UnityWebRequest(url, "POST");
                request.uploadHandler = new UploadHandlerRaw(new UTF8Encoding().GetBytes(data));
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", contentType);
                await request.SendWebRequest();
                return UnityWebRequestToNetworkResponse(request);
            }));
        }

        public async ValueTask<NetworkResponse<TResponse>> Get<TResponse>(string url)
        {
            var response = await GetRaw(url);
            return response.Map(x => JsonConvert.DeserializeObject<TResponse>(x));
        }

        public async ValueTask<NetworkResponse<TResponse>> Post<TRequest, TResponse>(string url, TRequest data)
        {
            var requestJson = JsonConvert.SerializeObject(data);
            var response = await PostRaw(url, requestJson, "application/json");
            return response.Map(x => JsonConvert.DeserializeObject<TResponse>(x));
        }
    }
}