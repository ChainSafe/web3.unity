using System.Threading.Tasks;
using ChainSafe.Gaming.Web3.Environment;
using ChainSafe.Gaming.Web3.Environment.Http;

namespace ChainSafe.Gaming.Tests.Core
{
    public sealed class StubHttpClient : IHttpClient
    {
        public ValueTask<NetworkResponse<string>> GetRaw(string url, params HttpHeader[] headers)
        {
            // do nothing
            return new ValueTask<NetworkResponse<string>>(Task.FromResult(NetworkResponse<string>.Success("Skipped")));
        }

        public ValueTask<NetworkResponse<string>> PostRaw(
            string url,
            string data,
            string contentType,
            params HttpHeader[] headers)
        {
            // do nothing
            return new ValueTask<NetworkResponse<string>>(Task.FromResult(NetworkResponse<string>.Success("Skipped")));
        }

        public ValueTask<NetworkResponse<TResponse>> Get<TResponse>(string url, params HttpHeader[] headers)
        {
            // do nothing
            return new ValueTask<NetworkResponse<TResponse>>(Task.FromResult(NetworkResponse<TResponse>.Success(default)));
        }

        public ValueTask<NetworkResponse<TResponse>> Post<TRequest, TResponse>(
            string url,
            TRequest data,
            params HttpHeader[] headers)
        {
            // do nothing
            return new ValueTask<NetworkResponse<TResponse>>(Task.FromResult(NetworkResponse<TResponse>.Success(default)));
        }
    }
}