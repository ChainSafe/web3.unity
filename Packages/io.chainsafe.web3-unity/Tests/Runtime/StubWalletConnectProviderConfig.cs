namespace Tests.Runtime
{
    public class StubWalletConnectProviderConfig
    {
        public const string DefaultWalletAddress = "0xD5c8010ef6dff4c83B19C511221A7F8d1e5cFF44";

        public string WalletAddress { get; set; } = DefaultWalletAddress;
        public string StubResponse { get; set; }
    }
}