using ChainSafe.Gaming.Web3.Evm.Wallet;

namespace Tests.Runtime
{
    public class StubWalletConnectProviderConfig : IWalletProviderConfig
    {
        public const string DefaultWalletAddress = "0x55ffe9E30347266f02b9BdAe20aD3a86493289ea";
        public const string DefaultProjectId = "f4bff60eb260841f46b1c77588cd8acb";

        public string SignMessageRpcMethodName => "personal_sign";
        public string SignTypedMessageRpcMethodName => "eth_signTypedData";

        public string ProjectId { get; set; } = DefaultProjectId;
        public string WalletAddress { get; set; } = DefaultWalletAddress;
        public string StubResponse { get; set; }
    }
}