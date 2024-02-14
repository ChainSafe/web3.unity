namespace ChainSafe.Gaming.SygmaClient.Types
{
    public static class ConfigUrl
    {
        public static readonly string Devnet = "https://chainbridge-assets-stage.s3.us-east-2.amazonaws.com/shared-config-dev.json";
        public static readonly string Testnet = "https://chainbridge-assets-stage.s3.us-east-2.amazonaws.com/shared-config-test.json";
        public static readonly string Mainnet = "https://sygma-assets-mainnet.s3.us-east-2.amazonaws.com/shared-config-mainnet.json";
    }

    public static class IndexerUrl
    {
        public static readonly string Mainnet = "https://api.buildwithsygma.com";
        public static readonly string Testnet = "https://api.test.buildwithsygma.com";
    }

    public static class ExplorerUrl
    {
        public static readonly string MAINNET = "https://scan.buildwithsygma.com";
        public static readonly string TESTNET = "https://scan.test.buildwithsygma.com";
    }
}