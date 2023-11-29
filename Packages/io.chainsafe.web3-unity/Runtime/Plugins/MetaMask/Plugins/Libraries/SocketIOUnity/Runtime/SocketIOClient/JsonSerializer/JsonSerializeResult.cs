using System.Collections.Generic;

namespace MetaMask.SocketIOClient.JsonSerializer
{
    public class JsonSerializeResult
    {
        public string Json { get; set; }
        public IList<byte[]> Bytes { get; set; }
    }
}
