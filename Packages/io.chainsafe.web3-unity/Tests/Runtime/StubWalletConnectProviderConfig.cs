namespace Tests.Runtime
{
    public class StubWalletConnectProviderConfig
    {
        public const string DefaultWalletAddress = "0xD5c8010ef6dff4c83B19C511221A7F8d1e5cFF44";
        public const string DefaultProjectId = "f4bff60eb260841f46b1c77588cd8acb";

        public string ProjectId { get; set; } = DefaultProjectId;
        public string WalletAddress { get; set; } = DefaultWalletAddress;
        public string StubResponse { get; set; }
    }
}