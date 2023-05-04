using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Web3Unity.Scripts.Library.Ethers.RPC
{
    // TODO @Oleksandr: Put these methods into different interfaces
    // suggestions: INetworkRequestHandler, ILogWriter (don't use ILogger,
    // that name is used by the MS logging libs) and ITaskRuntime
    public interface IRpcEnvironment
    {
        // TODO: use a strongly typed Url request
        Task<NetworkResponse> GetAsync(string url);
        Task<NetworkResponse> PostAsync(string url, string requestBody, string contentType);

        string GetDefaultRpcUrl();

        void LogError(string message);
        void CaptureEvent(string eventName, Dictionary<string, object> properties);

        void RunOnForegroundThread(Action action);
    }

    public class NetworkResponse
    {
        public string Response { get; private set; }
        public string Error { get; private set; }
        public bool IsSuccess { get; private set; }

        public static NetworkResponse Success(string response)
        {
            return new NetworkResponse() { Response = response, IsSuccess = true };
        }

        public static NetworkResponse Failure(string error)
        {
            return new NetworkResponse() { Error = error, IsSuccess = false };
        }

        public void EnsureSuccess()
        {
            if (!IsSuccess)
            {
                throw new Exception(Error);
            }
        }
    }
}
