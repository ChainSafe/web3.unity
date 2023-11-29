using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;

using MetaMask.SocketIOClient.JsonSerializer;
using MetaMask.SocketIOClient.Messages;

namespace MetaMask.SocketIOClient.Transport
{
    public class HttpTransport : BaseTransport
    {
        public HttpTransport(IHttpPollingHandler pollingHandler,
            SocketIOOptions options,
            IJsonSerializer jsonSerializer) : base(options, jsonSerializer)
        {
            //_http = http;
            this._httpPollingHandler = pollingHandler;
            this._httpPollingHandler.TextObservable.Subscribe(this);
            this._httpPollingHandler.BytesObservable.Subscribe(this);
        }

        private string _httpUri;
        private CancellationTokenSource _pollingTokenSource;
        //private Dictionary<string, string> defaultHeaders = new Dictionary<string, string>();

        //readonly HttpClient _http;
        private readonly IHttpPollingHandler _httpPollingHandler;

        private void StartPolling(CancellationToken cancellationToken)
        {
            //var coroutine = StartPollingCoroutine(cancellationToken);
            //SocketIOCouroutineRunner.Instance.RunCoroutine(coroutine);

            Task.Factory.StartNew(async () =>
            {
                int retry = 0;
                while (!cancellationToken.IsCancellationRequested)
                {
                    if (!this._httpUri.Contains("&sid="))
                    {
                        await Task.Delay(20);
                        continue;
                    }
                    try
                    {
                        await this._httpPollingHandler.GetAsync(this._httpUri, CancellationToken.None).ConfigureAwait(false);
                    }
                    catch (Exception e)
                    {
                        retry++;
                        if (retry >= 3)
                        {
                            MessageSubject.OnError(e);
                            break;
                        }
                        await Task.Delay(100 * (int)Math.Pow(2, retry));
                    }
                }
            }, TaskCreationOptions.LongRunning);
        }

        private IEnumerator StartPollingCoroutine(CancellationToken cancellationToken)
        {
            int retry = 0;
            while (!cancellationToken.IsCancellationRequested)
            {
                if (!this._httpUri.Contains("&sid="))
                {
                    yield return new UnityEngine.WaitForSecondsRealtime(0.02f);
                    continue;
                }
                Task task = null;
                try
                {
                    task = this._httpPollingHandler.GetAsync(this._httpUri, CancellationToken.None);
                    task.ConfigureAwait(false);
                }
                catch (Exception e)
                {
                    retry++;
                    if (retry >= 3)
                    {
                        MessageSubject.OnError(e);
                        break;
                    }
                }
                yield return new UnityEngine.WaitUntil(() => task == null || task.IsCompleted);
                if (task == null || task.IsCompleted)
                {
                    yield return new UnityEngine.WaitForSecondsRealtime((100 * (int)Math.Pow(2, retry)) / 1000);
                }
            }
        }

        public override async Task ConnectAsync(Uri uri, CancellationToken cancellationToken)
        {
            //var req = new HttpRequestMessage(HttpMethod.Get, uri);
            // if (_options.ExtraHeaders != null)
            // {
            //     foreach (var item in _options.ExtraHeaders)
            //     {
            //         req.Headers.Add(item.Key, item.Value);
            //     }
            // }

            this._httpUri = uri.ToString();
            await this._httpPollingHandler.SendAsync(uri, new CancellationTokenSource(Options.ConnectionTimeout).Token).ConfigureAwait(false);
            if (this._pollingTokenSource != null)
            {
                this._pollingTokenSource.Cancel();
            }
            this._pollingTokenSource = new CancellationTokenSource();
            StartPolling(this._pollingTokenSource.Token);
        }

        public override Task DisconnectAsync(CancellationToken cancellationToken)
        {
            this._pollingTokenSource.Cancel();
            if (PingTokenSource != null)
            {
                PingTokenSource.Cancel();
            }
            return Task.CompletedTask;
        }

        public override void AddHeader(string key, string val)
        {
            //_http.DefaultRequestHeaders.Add(key, val);
            this._httpPollingHandler.SetDefaultHeader(key, val);
        }

        public override void Dispose()
        {
            base.Dispose();
            this._httpPollingHandler.Dispose();
        }

        public override async Task SendAsync(Payload payload, CancellationToken cancellationToken)
        {
            await this._httpPollingHandler.PostAsync(this._httpUri, payload.Text, cancellationToken);
            if (payload.Bytes != null && payload.Bytes.Count > 0)
            {
                await this._httpPollingHandler.PostAsync(this._httpUri, payload.Bytes, cancellationToken);
            }
        }

        protected override async Task OpenAsync(OpenedMessage msg)
        {
            //if (!_httpUri.Contains("&sid="))
            //{
            //}
            this._httpUri += "&sid=" + msg.Sid;
            await base.OpenAsync(msg);
        }
    }
}
