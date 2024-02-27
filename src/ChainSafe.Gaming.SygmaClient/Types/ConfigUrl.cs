namespace ChainSafe.Gaming.SygmaClient.Types
{
    public static class ConfigUrl
    {
        public const string Devnet = "https://chainbridge-assets-stage.s3.us-east-2.amazonaws.com/shared-config-dev.json";
        public const string Testnet = "https://chainbridge-assets-stage.s3.us-east-2.amazonaws.com/shared-config-test.json";
        public const string Mainnet = "https://sygma-assets-mainnet.s3.us-east-2.amazonaws.com/shared-config-mainnet.json";
    }

    public static class IndexerUrl
    {
        public const string Mainnet = "https://api.buildwithsygma.com";
        public const string Testnet = "https://api.test.buildwithsygma.com";
    }

    public static class ExplorerUrl
    {
        public const string Mainnet = "https://scan.buildwithsygma.com";
        public const string Testnet = "https://scan.test.buildwithsygma.com";
    }
}