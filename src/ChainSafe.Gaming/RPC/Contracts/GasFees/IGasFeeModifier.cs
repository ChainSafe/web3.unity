using Nethereum.Hex.HexTypes;

namespace ChainSafe.Gaming.Evm.Contracts.GasFees
{
    public interface IGasFeeModifier
    {
        public HexBigInteger ModifyGasFee(HexBigInteger gasFee);
    }
}