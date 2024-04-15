using ChainSafe.Gaming.SygmaClient.Dto;
using Nethereum.Hex.HexTypes;

namespace ChainSafe.Gaming.SygmaClient.Types
{
    public class SygmaTransferParams
    {
        public string SourceAddress { get; set; }

        public string DestinationAddress { get; set; }

        public uint DestinationChainId { get; set; }

        public ResourceType ResourceType { get; set; }

        public string TokenId { get; set; }

        public HexBigInteger Amount { get; set; }

        public string DestinationProviderUrl { get; set; }
    }
}