using ChainSafe.Gaming.Configuration;

namespace ChainSafe.Gaming.NetCore.Configuration
{
    public class ChainConfig : IChainConfig
    {
        public string ChainId { get; set; }

        public string Chain { get; set; }

        public string Network { get; set; }

        public string Rpc { get; set; }
    }
}