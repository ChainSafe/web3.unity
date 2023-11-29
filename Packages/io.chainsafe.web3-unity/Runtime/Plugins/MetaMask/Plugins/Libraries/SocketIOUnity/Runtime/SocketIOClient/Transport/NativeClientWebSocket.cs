using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NativeWebSocket;

namespace MetaMask.SocketIOClient.Transport
{
    public class NativeClientWebSocket : IClientWebSocket
    {

        private const int ReceiveChunkSize = 1024 * 8;
        private readonly int _eio;

#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
        private BackgroundWebSocket bws;
#else
        private WebSocket ws;
#endif

        private bool _disposed;
        private readonly Subject<string> _textSubject;
        private readonly Subject<byte[]> _bytesSubject;
        private readonly CancellationTokenSource _listenCancellation;
        private readonly SemaphoreSlim _sendLock;

        private Dictionary<string, string> defaultHeaders = new Dictionary<string, string>();

        public IObservable<string> TextObservable { get; }
        public IObservable<byte[]> BytesObservable { get; }

        public NativeClientWebSocket(int eio)
        {
            _disposed = false;
            this._eio = eio;
            this._textSubject = new Subject<string>();
            this._bytesSubject = new Subject<byte[]>();
            TextObservable = this._textSubject.AsObservable();
            BytesObservable = this._bytesSubject.AsObservable();
            //this.ws = new WebSocket();
            this._listenCancellation = new CancellationTokenSource();
            this._sendLock = new SemaphoreSlim(1, 1);
        }

        private void ListenOnUnityThread()
        {
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
            IWebSocket ws = this.bws;
#else
            IWebSocket ws = this.ws;
#endif
            ws.OnMessage += OnWebSocketBinaryMessageReceived;
            ws.OnTextMessage += OnWebSocketTextMessageReceived;
            ws.OnClose += OnWebSocketClose;
        }

        private void Listen()
        {
            UnityThread.executeInUpdate(ListenOnUnityThread);
        }

        private void OnWebSocketClose(WebSocketCloseCode closeCode)
        {
            if (!_disposed)
            {
                this._textSubject.OnError(new WebSocketException("Received a Close message: " + closeCode));
            }
        }

        private void OnWebSocketTextMessageReceived(string data)
        {
            if (!_disposed)
            {
                this._textSubject.OnNext(data);
            }
        }

        private void OnWebSocketBinaryMessageReceived(byte[] data)
        {
            if (!_disposed)
            {
                int count = data.Length;
                byte[] bytes;
                if (this._eio == 3)
                {
                    bytes = new byte[count - 1];
                    Buffer.BlockCopy(data, 1, bytes, 0, bytes.Length);
                }
                else
                {
                    bytes = new byte[count];
                    Buffer.BlockCopy(data, 0, bytes, 0, bytes.Length);
                }
                this._bytesSubject.OnNext(data);
            }
        }

        public Task ConnectAsync(Uri uri, CancellationToken cancellationToken)
        {
            UnityThread.executeInUpdate(() =>
            {
                ConnectOnUnityThread(uri.ToString());
            });
            return Task.CompletedTask;
        }

        private void ConnectOnUnityThread(string uri)
        {
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
            this.bws = new BackgroundWebSocket(uri.ToString());
            _ = this.bws.Connect();
#else
            this.ws = new WebSocket(uri.ToString(), this.defaultHeaders);
            WebSocketDispatcher.Instance.AddWebSocket(this.ws);
            _ = this.ws.Connect();
#endif
            ListenOnUnityThread();
        }

        public async Task DisconnectAsync(CancellationToken cancellationToken)
        {
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
            if (this.bws != null && this.bws.IsConnected) { 
                await this.bws.Close();
            }
#else
            await this.ws.Close();
            if (this.ws != null)
            {
                WebSocketDispatcher.Instance.RemoveWebSocket(this.ws);
            }
#endif
        }

        public Task SendAsync(byte[] bytes, TransportMessageType type, bool endOfMessage, CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource<bool>();
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
            SendOnUnityTHread(bytes, type, tcs);
#else
            UnityThread.executeInUpdate(() =>
            {
                SendOnUnityTHread(bytes, type, tcs);
            });
#endif
            return tcs.Task;
        }

        private async void SendOnUnityTHread(byte[] bytes, TransportMessageType type, TaskCompletionSource<bool> tcs)
        {
            switch (type)
            {
                case TransportMessageType.Text:
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
                    await this.bws.Send(Encoding.UTF8.GetString(bytes));
#else
                    await this.ws.SendText(Encoding.UTF8.GetString(bytes));
#endif
                    break;
                case TransportMessageType.Binary:
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
                    await this.bws.Send(bytes);
#else
                    await this.ws.Send(bytes);
#endif
                    break;
            }
            tcs.SetResult(true);
        }

        public void AddHeader(string key, string val)
        {
            this.defaultHeaders[key] = val;
        }

        public void Dispose()
        {
            this._textSubject.Dispose();
            this._bytesSubject.Dispose();
            _disposed = true;

#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
            if (this.bws != null)
            {
                this.bws.Close();
            }
#else
            if (this.ws != null)
            {
                WebSocketDispatcher.Instance.RemoveWebSocket(this.ws);
                _ = this.ws.Close();
            }
#endif
        }
    }
}
