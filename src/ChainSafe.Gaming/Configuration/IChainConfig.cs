namespace ChainSafe.Gaming.Configuration
{
    public interface IChainConfig
    {
        public string ChainId { get; }

        public string Chain { get; }

        public string Network { get; }

        public string Rpc { get; }
    }
}