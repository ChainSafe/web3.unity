namespace MetaMask.SocketIOClient.Transport
{

    [System.Serializable]
    public class UnityWebRequestException : System.Exception
    {
        public UnityWebRequestException() { }
        public UnityWebRequestException(string message) : base(message) { }
        public UnityWebRequestException(string message, System.Exception inner) : base(message, inner) { }
        protected UnityWebRequestException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

}