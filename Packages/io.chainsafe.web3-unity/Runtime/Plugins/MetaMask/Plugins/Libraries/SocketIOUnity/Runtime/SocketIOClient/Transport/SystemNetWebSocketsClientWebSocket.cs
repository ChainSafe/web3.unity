using System;
using System.Net.WebSockets;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MetaMask.SocketIOClient.Transport
{
    public class SystemNetWebSocketsClientWebSocket : IClientWebSocket
    {
        public SystemNetWebSocketsClientWebSocket(int eio)
        {
            this._eio = eio;
            this._textSubject = new Subject<string>();
            this._bytesSubject = new Subject<byte[]>();
            TextObservable = this._textSubject.AsObservable();
            BytesObservable = this._bytesSubject.AsObservable();
            this._ws = new ClientWebSocket();
            this._listenCancellation = new CancellationTokenSource();
            this._sendLock = new SemaphoreSlim(1, 1);
        }

        private const int ReceiveChunkSize = 1024 * 8;
        private readonly int _eio;
        private readonly ClientWebSocket _ws;
        private readonly Subject<string> _textSubject;
        private readonly Subject<byte[]> _bytesSubject;
        private readonly CancellationTokenSource _listenCancellation;
        private readonly SemaphoreSlim _sendLock;

        public IObservable<string> TextObservable { get; }
        public IObservable<byte[]> BytesObservable { get; }

        private void Listen()
        {
            Task.Factory.StartNew(async () =>
            {
                while (true)
                {
                    if (this._listenCancellation.IsCancellationRequested)
                    {
                        break;
                    }
                    var buffer = new byte[ReceiveChunkSize];
                    int count = 0;
                    WebSocketReceiveResult result = null;

                    while (this._ws.State == WebSocketState.Open)
                    {
                        var subBuffer = new byte[ReceiveChunkSize];
                        try
                        {
                            result = await this._ws.ReceiveAsync(new ArraySegment<byte>(subBuffer), CancellationToken.None).ConfigureAwait(false);

                            // resize
                            if (buffer.Length - count < result.Count)
                            {
                                Array.Resize(ref buffer, buffer.Length + result.Count);
                            }
                            Buffer.BlockCopy(subBuffer, 0, buffer, count, result.Count);
                            count += result.Count;
                            if (result.EndOfMessage)
                            {
                                break;
                            }
                        }
                        catch (Exception e)
                        {
                            this._textSubject.OnError(e);
                            break;
                        }
                    }

                    if (result == null)
                    {
                        break;
                    }

                    switch (result.MessageType)
                    {
                        case WebSocketMessageType.Text:
                            string text = Encoding.UTF8.GetString(buffer, 0, count);
                            this._textSubject.OnNext(text);
                            break;
                        case WebSocketMessageType.Binary:
                            byte[] bytes;
                            if (this._eio == 3)
                            {
                                bytes = new byte[count - 1];
                                Buffer.BlockCopy(buffer, 1, bytes, 0, bytes.Length);
                            }
                            else
                            {
                                bytes = new byte[count];
                                Buffer.BlockCopy(buffer, 0, bytes, 0, bytes.Length);
                            }
                            this._bytesSubject.OnNext(bytes);
                            break;
                        case WebSocketMessageType.Close:
                            this._textSubject.OnError(new WebSocketException("Received a Close message"));
                            break;
                    }
                }
            });
        }

        public async Task ConnectAsync(Uri uri, CancellationToken cancellationToken)
        {
            await this._ws.ConnectAsync(uri, cancellationToken);
            Listen();
        }

        public async Task DisconnectAsync(CancellationToken cancellationToken)
        {
            await this._ws.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, cancellationToken);
        }

        public async Task SendAsync(byte[] bytes, TransportMessageType type, bool endOfMessage, CancellationToken cancellationToken)
        {
            var msgType = WebSocketMessageType.Text;
            if (type == TransportMessageType.Binary)
            {
                msgType = WebSocketMessageType.Binary;
            }
            await this._ws.SendAsync(new ArraySegment<byte>(bytes), msgType, endOfMessage, cancellationToken).ConfigureAwait(false);
        }

        public void AddHeader(string key, string val)
        {
            this._ws.Options.SetRequestHeader(key, val);
        }

        public void Dispose()
        {
            this._textSubject.Dispose();
            this._bytesSubject.Dispose();
            this._ws.Dispose();
        }
    }
}
