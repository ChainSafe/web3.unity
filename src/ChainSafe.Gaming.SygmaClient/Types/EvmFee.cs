using ChainSafe.Gaming.SygmaClient.Dto;
using Nethereum.Hex.HexTypes;

namespace ChainSafe.Gaming.SygmaClient.Types
{
    public class EvmFee
    {
        public EvmFee(string handlerAddress, FeeHandlerType type)
        {
            Type = type;
            HandlerAddress = handlerAddress;
        }

        public HexBigInteger Fee { get; set; }

        public FeeHandlerType Type { get; }

        public string HandlerAddress { get; }

        public string TokenAddress { get; set; }

        public string FeeData { get; set; }
    }
}