using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace MetaMask.SocketIOClient
{
    public class SocketIOResponse
    {
        public SocketIOResponse(IList<JsonElement> array, SocketIO socket)
        {
            this._array = array;
            InComingBytes = new List<byte[]>();
            SocketIO = socket;
            PacketId = -1;
        }

        private readonly IList<JsonElement> _array;

        public List<byte[]> InComingBytes { get; }
        public SocketIO SocketIO { get; }
        public int PacketId { get; set; }

        public T GetValue<T>(int index = 0)
        {
            var element = GetValue(index);
            string json = element.GetRawText();
            return SocketIO.JsonSerializer.Deserialize<T>(json, InComingBytes);
        }

        public JsonElement GetValue(int index = 0) => this._array[index];

        public int Count => this._array.Count;

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append('[');
            foreach (var item in this._array)
            {
                builder.Append(item.GetRawText());
                if (this._array.IndexOf(item) < this._array.Count - 1)
                {
                    builder.Append(',');
                }
            }
            builder.Append(']');
            return builder.ToString();
        }

        public async Task CallbackAsync(params object[] data)
        {
            await SocketIO.ClientAckAsync(PacketId, CancellationToken.None, data).ConfigureAwait(false);
        }

        public async Task CallbackAsync(CancellationToken cancellationToken, params object[] data)
        {
            await SocketIO.ClientAckAsync(PacketId, cancellationToken, data).ConfigureAwait(false);
        }
    }
}
