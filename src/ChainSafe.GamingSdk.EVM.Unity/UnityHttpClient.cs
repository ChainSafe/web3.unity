using System.Threading.Tasks;
using ChainSafe.GamingSdk.Evm.Unity;
using ChainSafe.GamingWeb3.Environment;

namespace ChainSafe.GamingWeb3.Unity
{
    public class UnityHttpClient : IHttpClient
    {
        private IMainThreadRunner _mainThreadRunner;

        public UnityHttpClient(IMainThreadRunner mainThreadRunner)
        {
            _mainThreadRunner = mainThreadRunner;
        }

        public ValueTask<string> GetRaw(string url)
        {
            throw new System.NotImplementedException();
        }

        public ValueTask<string> PostRaw(string url, string data, string contentType)
        {
            throw new System.NotImplementedException();
        }

        public ValueTask<TResponse> Get<TResponse>(string url)
        {
            throw new System.NotImplementedException();
        }

        public ValueTask<TResponse> Post<TRequest, TResponse>(string url, TRequest data)
        {
            throw new System.NotImplementedException();
        }
        
        // public Task<NetworkResponse> GetAsync(string url) =>
        //     _dispatcher.EnqueueTask(async () =>
        //     {
        //         using var request = UnityWebRequest.Get(url);
        //         request.downloadHandler = new DownloadHandlerBuffer();
        //         await request.SendWebRequest();
        //
        //         Assert.AreNotEqual(request.result, UnityWebRequest.Result.InProgress);
        //
        //         if (request.result != UnityWebRequest.Result.Success)
        //         {
        //             return NetworkResponse.Failure(request.error);
        //         }
        //
        //         return NetworkResponse.Success(request.downloadHandler.text);
        //     });
        //
        // public Task<NetworkResponse> PostAsync(string url, string requestBody, string contentType) =>
        //     _dispatcher.EnqueueTask(async () =>
        //     {
        //         using var request = new UnityWebRequest(url, "POST");
        //         request.uploadHandler = new UploadHandlerRaw(new UTF8Encoding().GetBytes(requestBody));
        //         request.downloadHandler = new DownloadHandlerBuffer();
        //         request.SetRequestHeader("Content-Type", contentType);
        //         await request.SendWebRequest();
        //
        //         if (request.result != UnityWebRequest.Result.Success)
        //         {
        //             return NetworkResponse.Failure(request.error);
        //         }
        //
        //         return NetworkResponse.Success(request.downloadHandler.text);
        //     });
    }
}