using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using Reown.AppKit.Unity.Utils;

namespace Reown.AppKit.Unity.Http
{
    public abstract class HttpClientDecorator
    {
        public Task<HttpResponseContext> SendAsync(HttpRequestContext requestContext, CancellationToken cancellationToken, Func<HttpRequestContext, CancellationToken, Task<HttpResponseContext>> next)
        {
            if (requestContext == null)
                throw new ArgumentNullException(nameof(requestContext));

            cancellationToken.ThrowIfCancellationRequested();

            return SendAsyncCore(requestContext, cancellationToken, next);
        }

        protected abstract Task<HttpResponseContext> SendAsyncCore(HttpRequestContext requestContext, CancellationToken cancellationToken, Func<HttpRequestContext, CancellationToken, Task<HttpResponseContext>> next);
    }

    public class HttpRequestContext
    {
        private int _decoratorIndex;

        public IDictionary<string, string> RequestHeaders { get; }

        public string Path { get; }
        public string Method { get; }
        public string Body { get; }
        public string ContentType { get; set; }
        public DateTimeOffset Timestamp { get; private set; }
        private HttpClientDecorator[] Decorators { get; }

        public HttpRequestContext(string path, string method, string body, string contentType = null, IDictionary<string, string> requestHeaders = null, HttpClientDecorator[] decorators = null)
        {
            Path = path;
            Method = method;
            Body = body;
            ContentType = contentType;
            Decorators = decorators;
            RequestHeaders = requestHeaders ?? new Dictionary<string, string>();
            Timestamp = DateTimeOffset.UtcNow;
        }

        public void Reset(HttpClientDecorator currentFilter)
        {
            _decoratorIndex = Array.IndexOf(Decorators, currentFilter);
            RequestHeaders?.Clear();
            Timestamp = DateTimeOffset.UtcNow;
        }

        internal HttpClientDecorator GetNextDecorator()
        {
            return Decorators[_decoratorIndex++];
        }
    }

    public class HttpResponseContext
    {
        public byte[] Bytes { get; }
        public long StatusCode { get; }
        public Dictionary<string, string> ResponseHeaders { get; }

        public HttpResponseContext(byte[] bytes, long statusCode, Dictionary<string, string> responseHeaders)
        {
            Bytes = bytes;
            StatusCode = statusCode;
            ResponseHeaders = responseHeaders;
        }

        public T GetResponseAs<T>()
        {
            var utf8String = Encoding.UTF8.GetString(Bytes);
            if (Type.GetTypeCode(typeof(T)) == TypeCode.String)
                return (T)Convert.ChangeType(utf8String, typeof(T));
            return JsonConvert.DeserializeObject<T>(utf8String);
        }
    }

    public class UnityHttpClient : HttpClientDecorator
    {
        private readonly Uri _basePath;
        private readonly TimeSpan _timeout;
        private readonly HttpClientDecorator[] _decorators;
        private readonly Func<HttpRequestContext, CancellationToken, Task<HttpResponseContext>> _next;

        public UnityHttpClient(params HttpClientDecorator[] decorators) : this(null, TimeSpan.FromSeconds(5), decorators)
        {
        }

        public UnityHttpClient(Uri basePath, TimeSpan timeout, params HttpClientDecorator[] decorators)
        {
            _basePath = basePath;
            _timeout = timeout;
            _next = InvokeRecursive;
            _decorators = new HttpClientDecorator[decorators.Length + 1];
            Array.Copy(decorators, _decorators, decorators.Length);
            _decorators[^1] = this;
        }

        public UnityHttpClient(TimeSpan timeout, params HttpClientDecorator[] decorators) : this(null, timeout, decorators)
        {
        }

        private Task<HttpResponseContext> InvokeRecursive(HttpRequestContext context, CancellationToken cancellationToken)
        {
            return context
                .GetNextDecorator()
                .SendAsync(context, cancellationToken, _next);
        }

        public async Task PostAsync(string path, string value, IDictionary<string, string> parameters = null, IDictionary<string, string> headers = null)
        {
            path = path.AppendQueryString(parameters);

            var request = new HttpRequestContext(path, "POST", value, "application/json", headers, _decorators);
            await InvokeRecursive(request, CancellationToken.None);
        }

        public async Task<T> PostAsync<T>(string path, string value, IDictionary<string, string> parameters = null, IDictionary<string, string> headers = null)
        {
            path = path.AppendQueryString(parameters);

            var request = new HttpRequestContext(path, "POST", value, "application/json", headers, _decorators);
            var response = await InvokeRecursive(request, CancellationToken.None);
            return response.GetResponseAs<T>();
        }

        public async Task<T> GetAsync<T>(string path, IDictionary<string, string> parameters = null, IDictionary<string, string> headers = null)
        {
            path = path.AppendQueryString(parameters);

            var request = new HttpRequestContext(path, "GET", null, null, headers, _decorators);
            var response = await InvokeRecursive(request, CancellationToken.None);
            return response.GetResponseAs<T>();
        }

        public async Task<IDictionary<string, string>> HeadAsync(string path, IDictionary<string, string> parameters = null, IDictionary<string, string> headers = null)
        {
            path = path.AppendQueryString(parameters);

            var request = new HttpRequestContext(path, "HEAD", null, null, headers, _decorators);
            var response = await InvokeRecursive(request, CancellationToken.None);
            return response.ResponseHeaders;
        }

        protected override Task<HttpResponseContext> SendAsyncCore(HttpRequestContext requestContext, CancellationToken cancellationToken, Func<HttpRequestContext, CancellationToken, Task<HttpResponseContext>> next)
        {
            var url = _basePath != null
                ? new Uri(_basePath, requestContext.Path)
                : new Uri(requestContext.Path);

            var uwr = new UnityWebRequest(url, requestContext.Method)
            {
                downloadHandler = new DownloadHandlerBuffer()
            };

            if (requestContext.Method is UnityWebRequest.kHttpVerbPOST or UnityWebRequest.kHttpVerbPUT)
            {
                if (string.IsNullOrWhiteSpace(requestContext.Body))
                {
                    uwr.SetRequestHeader("Content-Type", string.IsNullOrWhiteSpace(requestContext.ContentType)
                        ? "application/json"
                        : requestContext.ContentType
                    );
                }
                else
                {
                    var bytes = Encoding.UTF8.GetBytes(requestContext.Body);
                    uwr.uploadHandler = new UploadHandlerRaw(bytes);
                    uwr.uploadHandler.contentType = requestContext.ContentType;
                }
            }

            foreach (var (key, value) in requestContext.RequestHeaders)
                uwr.SetRequestHeader(key, value);

            uwr.timeout = (int)_timeout.TotalSeconds;

            var tcs = new TaskCompletionSource<HttpResponseContext>();
            cancellationToken.Register(
                callback => ((TaskCompletionSource<HttpResponseContext>)callback).SetCanceled(),
                tcs);

            var handle = uwr.SendWebRequest();

            handle.completed += _ =>
            {
                if (uwr.result != UnityWebRequest.Result.Success)
                {
                    tcs.SetException(new Exception($"Failed to send web request: {uwr.error}. Url: {url.ToString()}")); // TODO: use custom ex type
                    uwr.Dispose();
                    return;
                }

                tcs.SetResult(new HttpResponseContext(uwr.downloadHandler.data, uwr.responseCode, uwr.GetResponseHeaders()));
                uwr.Dispose();
            };

            return tcs.Task;
        }
    }
}