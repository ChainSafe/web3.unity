using Nethereum.Hex.HexTypes;

namespace ChainSafe.Gaming.Evm.Contracts.GasFees
{
    public class ImRichGasFeeModifier : IGasFeeModifier
    {
        public HexBigInteger ModifyGasFee(HexBigInteger gasFee)
        {
            return new HexBigInteger(gasFee.Value * 2);
        }
    }
}