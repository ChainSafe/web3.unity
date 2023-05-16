using System;
using System.Threading.Tasks;

namespace ChainSafe.GamingWeb3.Environment
{
    public interface IHttpClient
    {
        ValueTask<NetworkResponse<string>> GetRaw(string url);

        ValueTask<NetworkResponse<string>> PostRaw(string url, string data, string contentType);

        ValueTask<NetworkResponse<TResponse>> Get<TResponse>(string url);

        ValueTask<NetworkResponse<TResponse>> Post<TRequest, TResponse>(string url, TRequest data);
    }

    public class NetworkResponse<T>
    {
        public T Response { get; private set; }

        public string Error { get; private set; }

        public bool IsSuccess { get; private set; }

        public static NetworkResponse<T> Success(T response)
        {
            return new() { Response = response, IsSuccess = true };
        }

        public static NetworkResponse<T> Failure(string error)
        {
            return new() { Error = error, IsSuccess = false };
        }

        public void EnsureSuccess()
        {
            if (!IsSuccess)
            {
                throw new Exception(Error);
            }
        }

        public T EnsureResponse()
        {
            EnsureSuccess();
            return Response;
        }

        internal NetworkResponse<TOther> Map<TOther>(Func<T, TOther> f)
        {
            if (IsSuccess)
            {
                return NetworkResponse<TOther>.Success(f(Response));
            }
            else
            {
                return NetworkResponse<TOther>.Failure(Error);
            }
        }
    }
}