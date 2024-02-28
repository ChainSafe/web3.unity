using ChainSafe.Gaming.SygmaClient.Dto;

namespace ChainSafe.Gaming.SygmaClient.Types
{
    public struct BaseTransferParams
    {
        public EvmConfig SourceDomain;
        public EvmConfig DestinationDomain;
        public EvmResource Resource;
    }
}