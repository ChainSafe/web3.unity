namespace Plugins.CountlySDK.Helpers
{
    public struct CountlyResponse
    {
        public int StatusCode { get; set; }
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public string Data { get; set; }

        public override string ToString()
        {
            return $"{nameof(StatusCode)}: {StatusCode}, {nameof(IsSuccess)}: {IsSuccess}, {nameof(ErrorMessage)}: {ErrorMessage}, {nameof(Data)}: {Data}";
        }
    }
}
