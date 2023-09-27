namespace ChainSafe.Gaming.Web3.Environment
{
    public interface IOperatingSystemMediator
    {
        public string ClipboardContent { get; set; }

        public void OpenUrl(string url);
    }
}