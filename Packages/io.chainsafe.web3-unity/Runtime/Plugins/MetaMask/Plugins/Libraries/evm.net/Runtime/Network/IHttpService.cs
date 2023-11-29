using System.Threading.Tasks;

namespace evm.net.Network
{
    /// <summary>
    /// An interface for making simple HTTP/HTTPS requests.
    /// </summary>
    public interface IHttpService
    {
        /// <summary>
        /// Perform a GET request at the given URL. This returns the response as a string.
        /// </summary>
        /// <param name="uri">The URL to perform the GET request to</param>
        /// <returns>The response as a string</returns>
        Task<string> Get(string uri);

        /// <summary>
        /// Perform a POST request at the given URL. This returns the response as a string.
        /// </summary>
        /// <param name="uri">The URL to perform the POST request to</param>
        /// <param name="params">The raw string to place in the POST body</param>
        /// <returns>The response as a string</returns>
        Task<string> Post(string uri, string @params);
        
        /// <summary>
        /// Perform a DELETE request at the given URL. This returns the response as a string.
        /// </summary>
        /// <param name="uri">The URL to perform the DELETE request to</param>
        /// <param name="params">The raw string to place in the DELETE body</param>
        /// <returns>The response as a string</returns>
        Task<string> Delete(string uri, string @params);
    }
}