using Nethereum.Hex.HexTypes;

namespace ChainSafe.Gaming.Evm.Contracts.GasFees
{
    public class BareMinimumGasFeeModifier : IGasFeeModifier
    {
        public HexBigInteger ModifyGasFee(HexBigInteger gasFee)
        {
            return new HexBigInteger(gasFee.Value + 1);
        }
    }
}