using System.IO;
using System.Text;
using System.Threading.Tasks;
using ChainSafe.Gaming.Unity;
using ChainSafe.Gaming.Web3.Environment;
using Newtonsoft.Json;
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

        /// <summary>
        /// Converts the UnityWebRequest response to a structured NetworkResponse.
        /// </summary>
        /// <param name="request">The Unity web request instance.</param>
        /// <returns>Converted Network Response.</returns>
        private static NetworkResponse<string> UnityWebRequestToNetworkResponse(UnityWebRequest request)
        {
            Assert.AreNotEqual(request.result, UnityWebRequest.Result.InProgress);

            if (request.result != UnityWebRequest.Result.Success)
            {
                throw new Web3Exception($"HTTP.{request.method} to {request.url} responded with error: {request.downloadHandler.text}");
            }

            return NetworkResponse<string>.Success(request.downloadHandler.text);
        }

        /// <summary>
        /// Sends a GET request to the specified URL and retrieves the raw response.
        /// </summary>
        /// <param name="url">The target URL.</param>
        /// <returns>A value task that completes with the network response.</returns>
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

        /// <summary>
        /// Sends a POST request to the specified URL with the provided raw data.
        /// </summary>
        /// <param name="url">The target URL.</param>
        /// <param name="data">The raw data to be sent as the request body.</param>
        /// <param name="contentType">The MIME type of the request data.</param>
        /// <returns>A value task that completes with the network response.</returns>
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

        /// <summary>
        /// Sends a GET request to the specified URL and deserializes the response into the desired type.
        /// </summary>
        /// <typeparam name="TResponse">The type to deserialize the response into.</typeparam>
        /// <param name="url">The target URL.</param>
        /// <returns>A value task that completes with the network response.</returns>
        public async ValueTask<NetworkResponse<TResponse>> Get<TResponse>(string url)
        {
            var response = await GetRaw(url);
            return response.Map(x => JsonConvert.DeserializeObject<TResponse>(x));
        }

        /// <summary>
        /// Sends a POST request to the specified URL with the provided data and deserializes the response into the desired type.
        /// </summary>
        /// <typeparam name="TRequest">The type of the request data.</typeparam>
        /// <typeparam name="TResponse">The type to deserialize the response into.</typeparam>
        /// <param name="url">The target URL.</param>
        /// <param name="data">The data to be sent as the request body.</param>
        /// <returns>A value task that completes with the network response.</returns>
        public async ValueTask<NetworkResponse<TResponse>> Post<TRequest, TResponse>(string url, TRequest data)
        {
            var requestJson = JsonConvert.SerializeObject(data);
            var response = await PostRaw(url, requestJson, "application/json");
            return response.Map(x => JsonConvert.DeserializeObject<TResponse>(x));
        }
    }
}
