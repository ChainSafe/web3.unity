using System;
using System.Collections;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using UnityEngine.Networking;

namespace MetaMask.SocketIOClient.Transport
{
    public abstract class HttpPollingHandler : IHttpPollingHandler
    {
        public HttpPollingHandler()
        {
            //HttpClient = httpClient;
            TextSubject = new Subject<string>();
            BytesSubject = new Subject<byte[]>();
            TextObservable = TextSubject.AsObservable();
            BytesObservable = BytesSubject.AsObservable();
        }

        protected Dictionary<string, string> defaultHeaders = new Dictionary<string, string>();

        //protected HttpClient HttpClient { get; }
        protected Subject<string> TextSubject { get; }
        protected Subject<byte[]> BytesSubject { get; }

        public IObservable<string> TextObservable { get; }
        public IObservable<byte[]> BytesObservable { get; }

        public void SetDefaultHeader(string key, string val)
        {
            this.defaultHeaders[key] = val;
        }

        protected void ApplyDefaultHeaders(UnityWebRequest uwr)
        {
            foreach (var item in this.defaultHeaders)
            {
                uwr.SetRequestHeader(item.Key, item.Value);
            }
        }

        protected void PrepareUnityWebRequest(UnityWebRequest uwr)
        {
            ApplyDefaultHeaders(uwr);
            uwr.disposeDownloadHandlerOnDispose = true;
            uwr.disposeUploadHandlerOnDispose = true;
        }

        protected string AppendTimestamp(string uri)
        {
            return uri + "&t=" + DateTimeOffset.Now.ToUnixTimeSeconds();
        }

        public Task GetAsync(string uri, CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource<bool>();
            UnityThread.executeCoroutine(GetCoroutine(uri, tcs));
            //var req = new HttpRequestMessage(HttpMethod.Get, AppendRandom(uri));
            //var resMsg = await HttpClient.SendAsync(req, cancellationToken).ConfigureAwait(false);
            //if (!resMsg.IsSuccessStatusCode)
            //{
            //    throw new HttpRequestException($"Response status code does not indicate success: {resMsg.StatusCode}");
            //}
            //ProduceMessageAsync(resMsg);
            return tcs.Task;
        }

        private IEnumerator GetCoroutine(string uri, TaskCompletionSource<bool> tcs)
        {
            string requestUri = AppendTimestamp(uri);
            var uwr = UnityWebRequest.Get(requestUri);
            PrepareUnityWebRequest(uwr);
            yield return uwr.SendWebRequest();

            if (uwr.result != UnityWebRequest.Result.Success)
            {
                string error = $"Response status code does not indicate success: {uwr.responseCode}";
                uwr.Dispose();
                tcs.SetException(new UnityWebRequestException(error));
                throw new UnityWebRequestException(error);
            }
            ProduceMessageAsync(uwr);
            tcs.SetResult(true);
        }

        public Task SendAsync(Uri uri, CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource<bool>();
            UnityThread.executeCoroutine(SendCoroutine(uri.ToString(), tcs));
            //var resMsg = await HttpClient.SendAsync(req, cancellationToken).ConfigureAwait(false);
            //if (!resMsg.IsSuccessStatusCode)
            //{
            //    throw new HttpRequestException($"Response status code does not indicate success: {resMsg.StatusCode}");
            //}
            //ProduceMessageAsync(resMsg);
            return tcs.Task;
        }

        private IEnumerator SendCoroutine(string uri, TaskCompletionSource<bool> tcs)
        {
            var uwr = UnityWebRequest.Get(uri);
            PrepareUnityWebRequest(uwr);
            yield return uwr.SendWebRequest();

            if (uwr.result != UnityWebRequest.Result.Success)
            {
                string error = $"Response status code does not indicate success: {uwr.responseCode}";
                uwr.Dispose();
                tcs.SetException(new UnityWebRequestException(error));
                throw new UnityWebRequestException(error);
            }
            ProduceMessageAsync(uwr);
            tcs.SetResult(true);
        }

        public virtual Task PostAsync(string uri, string content, CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource<bool>();
            UnityThread.executeCoroutine(PostCoroutine(uri, content, tcs));

            //var httpContent = new StringContent(content);
            //var resMsg = await HttpClient.PostAsync(AppendTimestamp(uri), httpContent, cancellationToken).ConfigureAwait(false);
            //ProduceMessageAsync(resMsg);
            return tcs.Task;
        }

        private IEnumerator PostCoroutine(string uri, string content, TaskCompletionSource<bool> tcs)
        {
            string requestUri = AppendTimestamp(uri);
            var downloadHandler = new DownloadHandlerBuffer();
            var uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(content));
            var uwr = new UnityWebRequest(requestUri, UnityWebRequest.kHttpVerbPOST, downloadHandler, uploadHandler);
            PrepareUnityWebRequest(uwr);
            yield return uwr.SendWebRequest();

            if (uwr.result != UnityWebRequest.Result.Success)
            {
                string error = $"Response status code does not indicate success: {uwr.responseCode}";
                uwr.Dispose();
                tcs.SetException(new UnityWebRequestException(error));
                throw new UnityWebRequestException(error);
            }
            ProduceMessageAsync(uwr);
            tcs.SetResult(true);
        }

        public abstract Task PostAsync(string uri, IEnumerable<byte[]> bytes, CancellationToken cancellationToken);

        private void ProduceMessageAsync(UnityWebRequest uwr)
        {
            //if (resMsg.Content.Headers.ContentType.MediaType == "application/octet-stream")
            //{
            //    byte[] bytes = await resMsg.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
            //    ProduceBytes(bytes);
            //}
            //else
            //{
            //    string text = await resMsg.Content.ReadAsStringAsync().ConfigureAwait(false);
            //    ProduceText(text);
            //}

            if (uwr.GetRequestHeader("Content-Type") == "application/octet-stream")
            {
                ProduceBytes(uwr.downloadHandler.data);
            }
            else
            {
                ProduceText(uwr.downloadHandler.text);
            }
            uwr.Dispose();
        }

        protected abstract void ProduceText(string text);

        private void ProduceBytes(byte[] bytes)
        {
            int i = 0;
            while (bytes.Length > i + 4)
            {
                byte type = bytes[i];
                var builder = new StringBuilder();
                i++;
                while (bytes[i] != byte.MaxValue)
                {
                    builder.Append(bytes[i]);
                    i++;
                }
                i++;
                int length = int.Parse(builder.ToString());
                if (type == 0)
                {
                    var buffer = new byte[length];
                    Buffer.BlockCopy(bytes, i, buffer, 0, buffer.Length);
                    TextSubject.OnNext(Encoding.UTF8.GetString(buffer));
                }
                else if (type == 1)
                {
                    var buffer = new byte[length - 1];
                    Buffer.BlockCopy(bytes, i + 1, buffer, 0, buffer.Length);
                    BytesSubject.OnNext(buffer);
                }
                i += length;
            }
        }

        public void Dispose()
        {
            TextSubject.Dispose();
            BytesSubject.Dispose();
        }
    }
}
