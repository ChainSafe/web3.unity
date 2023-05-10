using System.Threading.Tasks;

namespace ChainSafe.GamingWeb3.Environment
{
    public interface IHttpClient
    {
        ValueTask<string> GetRaw(string url);
        ValueTask<string> PostRaw(string url, string data, string contentType);

        ValueTask<TResponse> Get<TResponse>(string url);
        ValueTask<TResponse> Post<TRequest, TResponse>(string url, TRequest data);
    }
}