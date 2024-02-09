using System.Threading.Tasks;
using ChainSafe.Gaming.Web3.Environment;

namespace ChainSafe.Gaming.Tests.Core
{
    public sealed class StubHttpClient : IHttpClient
    {
        public ValueTask<NetworkResponse<string>> GetRaw(string url)
        {
            // do nothing
            return new ValueTask<NetworkResponse<string>>(Task.FromResult(NetworkResponse<string>.Success("Skipped")));
        }

        public ValueTask<NetworkResponse<string>> PostRaw(string url, string data, string contentType)
        {
            // do nothing
            return new ValueTask<NetworkResponse<string>>(Task.FromResult(NetworkResponse<string>.Success("Skipped")));
        }

        public ValueTask<NetworkResponse<TResponse>> Get<TResponse>(string url)
        {
            // do nothing
            return new ValueTask<NetworkResponse<TResponse>>(Task.FromResult(NetworkResponse<TResponse>.Success(default)));
        }

        public ValueTask<NetworkResponse<TResponse>> Post<TRequest, TResponse>(string url, TRequest data)
        {
            // do nothing
            return new ValueTask<NetworkResponse<TResponse>>(Task.FromResult(NetworkResponse<TResponse>.Success(default)));
        }
    }
}