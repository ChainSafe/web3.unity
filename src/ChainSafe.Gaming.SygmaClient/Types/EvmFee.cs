using Nethereum.Hex.HexTypes;

namespace ChainSafe.Gaming.SygmaClient.Types
{
    public class EvmFee
    {
        public EvmFee(HexBigInteger fee, FeeHandlerType type, string handlerAddress, string tokenAddress, string feeData)
        {
            Fee = fee;
            Type = type;
            HandlerAddress = handlerAddress;
            TokenAddress = tokenAddress;
            FeeData = feeData;
        }

        public HexBigInteger Fee { get; }

        public FeeHandlerType Type { get; }

        public string HandlerAddress { get; }

        public string TokenAddress { get; }

        public string FeeData { get; }
    }
}