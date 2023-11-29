using System;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using MetaMask.SocketIOClient.JsonSerializer;

namespace MetaMask.SocketIOClient.Transport
{
    public class WebSocketTransport : BaseTransport
    {
        public WebSocketTransport(IClientWebSocket ws, SocketIOOptions options, IJsonSerializer jsonSerializer) : base(options, jsonSerializer)
        {
            this._ws = ws;
            this._sendLock = new SemaphoreSlim(1, 1);
            this._ws.TextObservable.Subscribe(this);
            this._ws.BytesObservable.Subscribe(this);
        }

        private const int ReceiveChunkSize = 1024 * 8;
        private const int SendChunkSize = 1024 * 8;
        private readonly IClientWebSocket _ws;
        private readonly SemaphoreSlim _sendLock;

        private async Task SendAsync(TransportMessageType type, byte[] bytes, CancellationToken cancellationToken)
        {
            try
            {
                await this._sendLock.WaitAsync().ConfigureAwait(false);
                if (type == TransportMessageType.Binary && Options.EIO == 3)
                {
                    byte[] buffer = new byte[bytes.Length + 1];
                    buffer[0] = 4;
                    Buffer.BlockCopy(bytes, 0, buffer, 1, bytes.Length);
                    bytes = buffer;
                }
                int pages = (int)Math.Ceiling(bytes.Length * 1.0 / SendChunkSize);
                for (int i = 0; i < pages; i++)
                {
                    int offset = i * SendChunkSize;
                    int length = SendChunkSize;
                    if (offset + length > bytes.Length)
                    {
                        length = bytes.Length - offset;
                    }
                    byte[] subBuffer = new byte[length];
                    Buffer.BlockCopy(bytes, offset, subBuffer, 0, subBuffer.Length);
                    bool endOfMessage = pages - 1 == i;
                    await this._ws.SendAsync(subBuffer, type, endOfMessage, cancellationToken).ConfigureAwait(false);
                }
            }
            finally
            {
                this._sendLock.Release();
            }
        }

        public override async Task ConnectAsync(Uri uri, CancellationToken cancellationToken)
        {
            await this._ws.ConnectAsync(uri, cancellationToken);
        }

        public override async Task DisconnectAsync(CancellationToken cancellationToken)
        {
            try
            {
                await this._ws.DisconnectAsync(cancellationToken);
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogException(e);
            }
        }

        public override async Task SendAsync(Payload payload, CancellationToken cancellationToken)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(payload.Text);
            await SendAsync(TransportMessageType.Text, bytes, cancellationToken);
            if (payload.Bytes != null)
            {
                foreach (var item in payload.Bytes)
                {
                    await SendAsync(TransportMessageType.Binary, item, cancellationToken);
                }
            }
        }

        public override void AddHeader(string key, string val) => this._ws.AddHeader(key, val);

        public override void Dispose()
        {
            this._ws.Dispose();
            base.Dispose();
            this._sendLock.Dispose();
        }
    }
}
