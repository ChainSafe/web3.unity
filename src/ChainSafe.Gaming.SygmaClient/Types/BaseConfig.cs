using System.Collections.Generic;

namespace ChainSafe.Gaming.SygmaClient.Types
{
    public class BaseConfig
    {
        protected BaseConfig(uint id, uint chainId, string name, Network type, string bridge)
        {
            Id = id;
            ChainId = chainId;
            Name = name;
            Type = type;
            Bridge = bridge;
        }

        public uint Id { get; }

        public uint ChainId { get; }

        public string Name { get; }

        public Network Type { get; }

        public string Bridge { get; }

        public string NativeTokenSymbol { get; set; }

        public string NativeTokenFullName { get; set; }

        public long NativeTokenDecimals { get; set; }

        public long StartBlock { get; set; }

        public int BlockConfirmations { get; set; }

        public List<EvmResource> Resources { get; set; }
    }

    public class EvmConfig : BaseConfig
    {
        public EvmConfig(uint id, uint chainId, string name, Network type, string bridge)
            : base(id, chainId, name, type, bridge)
        {
        }

        public List<Handler> Handlers { get; set; }

        public string FeeRouter { get; set; }

        public List<FeeHandler> FeeHandlers { get; set; }
    }
}