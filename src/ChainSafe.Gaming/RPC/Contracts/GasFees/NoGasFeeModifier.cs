using Nethereum.Hex.HexTypes;

namespace ChainSafe.Gaming.Evm.Contracts.GasFees
{
    public class NoGasFeeModifier : IGasFeeModifier
    {
        public HexBigInteger ModifyGasFee(HexBigInteger gasFee)
        {
            return gasFee;
        }
    }
}