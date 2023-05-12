using Web3Unity.Scripts.Library.Ethers.RPC;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Web3Unity.Scripts.Library.Ethers.NetCore
{
    public class NetCoreRpcEnvironment : IRpcEnvironment
    {
        private readonly string _defaultRpcUrl;

        public NetCoreRpcEnvironment(string defaultRpcUrl)
        {
            _defaultRpcUrl = defaultRpcUrl;
        }

        public static void InitializeRpcEnvironment(string defaultRpcUrl)
        {
            RpcEnvironmentStore.Initialize(new NetCoreRpcEnvironment(defaultRpcUrl));
        }

        public string GetDefaultRpcUrl() => _defaultRpcUrl;

        public void LogError(string message)
        {
            // TODO: An ILogger would be a much better choice here.
            Console.WriteLine(message);
        }

        public async Task<NetworkResponse> GetAsync(string url)
        {
            using var httpClient = new HttpClient();
            using var response = await httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                return NetworkResponse.Failure($"{response.StatusCode} {response.ReasonPhrase}");
            }

            return NetworkResponse.Success(await response.Content.ReadAsStringAsync());
        }

        public async Task<NetworkResponse> PostAsync(string url, string requestBody, string contentType)
        {
            using var httpClient = new HttpClient();
            var content = new StringContent(requestBody, Encoding.UTF8, contentType);
            using var response = await httpClient.PostAsync(url, content);

            if (!response.IsSuccessStatusCode)
            {
                return NetworkResponse.Failure($"{response.StatusCode} {response.ReasonPhrase}");
            }

            return NetworkResponse.Success(await response.Content.ReadAsStringAsync());
        }

        public void RunOnForegroundThread(Action action)
        {
            // There is no "foreground" thread in a normal .NET app (unless
            // it's a UI app or something), so just run the action on the
            // thread pool to make whatever it does non-blocking.
            Task.Run(() =>
            {
                try
                {
                    action();
                }
                catch (Exception e)
                {
                    LogError(e.ToString());
                }
            });
        }

        // TODO: find suitable event capturing solution.
        public void CaptureEvent(string eventName, Dictionary<string, object> properties) { }
    }
}
