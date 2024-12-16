using System;
using System.Threading.Tasks;
using ChainSafe.Gaming.Web3.Environment.Http;
using Newtonsoft.Json;

namespace ChainSafe.Gaming.Web3.Environment
{
    /// <summary>
    /// Interface for HTTP Client to be used by the SDK.
    /// </summary>
    public interface IHttpClient
    {
        /// <summary>
        /// Makes a GET request.
        /// </summary>
        /// <param name="url">URL to send request to.</param>
        /// <param name="headers">Set of headers to use with the HTTP request.</param>
        /// <returns>Server response.</returns>
        ValueTask<NetworkResponse<string>> GetRaw(string url, params HttpHeader[] headers);

        /// <summary>
        /// Makes a POST request.
        /// </summary>
        /// <param name="url">URL to send request to.</param>
        /// <param name="data">Data to send.</param>
        /// <param name="contentType">Content type of the data (ex. 'application/json').</param>
        /// <param name="headers">Set of headers to use with the HTTP request.</param>
        /// <returns>Server response.</returns>
        ValueTask<NetworkResponse<string>> PostRaw(
            string url,
            string data,
            string contentType,
            params HttpHeader[] headers);

        /// <summary>
        /// Makes a GET request. Deserializes response from JSON to the specified type.
        /// </summary>
        /// <param name="url">URL to send request to.</param>
        /// <param name="headers">Set of headers to use with the HTTP request.</param>
        /// <typeparam name="TResponse">Type of the response data.</typeparam>
        /// <returns>Server response.</returns>
        async ValueTask<NetworkResponse<TResponse>> Get<TResponse>(string url, params HttpHeader[] headers)
        {
            var response = await GetRaw(url, headers);
            return response.Map(x =>
            {
                try
                {
                    return JsonConvert.DeserializeObject<TResponse>(x);
                }
                catch (JsonReaderException e)
                {
                    throw new Web3Exception($"Tried deserializing response, but failed.\nMessage:{e.Message}\nResponse body:\n{x}");
                }
            });
        }

        /// <summary>
        /// Makes a POST request. Handles JSON serialization/deserialization of request and response for the specified types.
        /// </summary>
        /// <param name="url">URL to send request to.</param>
        /// <param name="data">Data object to send.</param>
        /// <param name="headers">Set of headers to use with the HTTP request.</param>
        /// <typeparam name="TRequest">Type of content used for request.</typeparam>
        /// <typeparam name="TResponse">Type of content expected as the response.</typeparam>
        /// <returns>Server response.</returns>
        async ValueTask<NetworkResponse<TResponse>> Post<TRequest, TResponse>(
            string url,
            TRequest data,
            params HttpHeader[] headers)
        {
            var requestJson = JsonConvert.SerializeObject(data);
            var response = await PostRaw(url, requestJson, "application/json", headers);
            return response.Map(x =>
            {
                try
                {
                    return JsonConvert.DeserializeObject<TResponse>(x);
                }
                catch (JsonReaderException e)
                {
                    throw new Web3Exception($"Tried deserializing response, but failed.\nMessage:{e.Message}\nResponse body:\n{x}");
                }
            });
        }
    }

    /// <summary>
    /// Represents server response.
    /// </summary>
    /// <typeparam name="T">Type of content expected as the response.</typeparam>
    public class NetworkResponse<T>
    {
        /// <summary>
        /// Response body.
        /// </summary>
        public T Response { get; private set; }

        /// <summary>
        /// Request error.
        /// </summary>
        public string Error { get; private set; }

        /// <summary>
        /// Was the request successful.
        /// </summary>
        public bool IsSuccess { get; private set; }

        /// <summary>
        /// Shorthand to create a new instance of response for a successful request.
        /// </summary>
        /// <param name="response">Response body.</param>
        /// <returns>New instance of <see cref="NetworkResponse{T}"/>.</returns>
        public static NetworkResponse<T> Success(T response)
        {
            return new() { Response = response, IsSuccess = true };
        }

        /// <summary>
        /// Shorthand to create a new instance of response for a failed request.
        /// </summary>
        /// <param name="error">Error message.</param>
        /// <returns>New instance of <see cref="NetworkResponse{T}"/>.</returns>
        public static NetworkResponse<T> Failure(string error)
        {
            return new() { Error = error, IsSuccess = false };
        }

        /// <summary>
        /// Assert that the request was successful.
        /// </summary>
        /// <returns>Response object to enable fluent syntax.</returns>
        /// <exception cref="Web3Exception">Request was not successful.</exception>
        public T AssertSuccess()
        {
            if (!IsSuccess)
            {
                throw new Web3Exception(Error);
            }

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