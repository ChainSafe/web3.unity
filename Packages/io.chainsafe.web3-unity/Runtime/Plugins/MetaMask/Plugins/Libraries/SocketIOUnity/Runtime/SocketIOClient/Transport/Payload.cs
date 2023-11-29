using System.Collections.Generic;

namespace MetaMask.SocketIOClient.Transport
{
    public class Payload
    {
        public string Text { get; set; }
        public List<byte[]> Bytes { get; set; }
    }
}
