using ChainSafe.Gaming.SygmaClient.Dto;

namespace ChainSafe.Gaming.SygmaClient.Types
{
    public enum Environment
    {
        Local,
        Devnet,
        Testnet,
        Mainnet,
    }

    public class Domain
    {
        public Domain(uint id, uint chainId, string name, Network type)
        {
            Id = id;
            ChainId = chainId;
            Name = name;
            Type = type;
        }

        public uint Id { get; }

        public uint ChainId { get; }

        public string Name { get; }

        public Network Type { get; }
    }
}