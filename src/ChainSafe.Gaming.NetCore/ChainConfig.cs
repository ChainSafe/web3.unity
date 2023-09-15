using ChainSafe.Gaming.Web3;

namespace ChainSafe.Gaming.NetCore
{
    public class ChainConfig : IChainConfig
    {
        public string ChainId { get; set; }

        public string Chain { get; set; }

        public string Network { get; set; }

        public string Rpc { get; set; }

        public string Ipc { get; set; }

        public string Ws { get; set; }
    }
}