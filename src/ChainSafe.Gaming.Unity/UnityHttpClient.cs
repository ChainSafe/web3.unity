using System.Text;
using System.Threading.Tasks;
using ChainSafe.Gaming.Web3.Environment;
using ChainSafe.Gaming.Web3.Environment.Http;
using UnityEngine.Assertions;
using UnityEngine.Networking;

namespace ChainSafe.Gaming.Web3.Core.Unity
{
    /// <summary>
    /// A Unity-specific implementation of an HTTP client, allowing for making web requests within the Unity environment.
    /// This class handles both GET and POST requests and ensures operations occur on the main Unity thread.
    /// </summary>
    public class UnityHttpClient : IHttpClient
    {
        private readonly IMainThreadRunner mainThreadRunner;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnityHttpClient"/> class.
        /// </summary>
        /// <param name="mainThreadRunner">Provides capabilities to run tasks on the Unity main thread.</param>
        public UnityHttpClient(IMainThreadRunner mainThreadRunner)
        {
            this.mainThreadRunner = mainThreadRunner;
        }

        public ValueTask<NetworkResponse<string>> GetRaw(string url, params HttpHeader[] headers)
        {
            return new ValueTask<NetworkResponse<string>>(mainThreadRunner.EnqueueTask(async () =>
            {
                using var request = UnityWebRequest.Get(url);

                foreach (var header in headers)
                {
                    request.SetRequestHeader(header.Name, header.Value);
                }

                request.downloadHandler = new DownloadHandlerBuffer();
                await request.SendWebRequest();
                return UnityWebRequestToNetworkResponse(request);
            }));
        }

        public ValueTask<NetworkResponse<string>> PostRaw(
            string url,
            string data,
            string contentType,
            params HttpHeader[] headers)
        {
            return new ValueTask<NetworkResponse<string>>(mainThreadRunner.EnqueueTask(async () =>
            {
                using var request = new UnityWebRequest(url, "POST");

                foreach (var header in headers)
                {
                    request.SetRequestHeader(header.Name, header.Value);
                }

                request.uploadHandler = new UploadHandlerRaw(new UTF8Encoding().GetBytes(data));
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", contentType);
                await request.SendWebRequest();
                return UnityWebRequestToNetworkResponse(request);
            }));
        }

        /// <summary>
        /// Converts the UnityWebRequest response to a structured NetworkResponse.
        /// </summary>
        /// <param name="request">The Unity web request instance.</param>
        /// <returns>Converted Network Response.</returns>
        private static NetworkResponse<string> UnityWebRequestToNetworkResponse(UnityWebRequest request)
        {
            // Assert response is successful
            {
                Assert.AreNotEqual(UnityWebRequest.Result.InProgress, request.result);

                if (request.result == UnityWebRequest.Result.ConnectionError)
                {
                    throw new Web3Exception($"HTTP.{request.method} to '{request.url}' - connection error : {request.error}.");
                }

                if (request.result != UnityWebRequest.Result.Success)
                {
                    throw new Web3Exception(
                        $"HTTP.{request.method} to '{request.url}' responded with error: {request.downloadHandler.text}");
                }
            }

            return NetworkResponse<string>.Success(request.downloadHandler.text);
        }
    }
}
