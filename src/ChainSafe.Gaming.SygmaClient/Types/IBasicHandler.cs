using System.Threading.Tasks;
using Nethereum.Hex.HexTypes;

namespace ChainSafe.Gaming.SygmaClient.Types
{
    public interface IBasicHandler
    {
        Task<EvmFee> CalculateBasicFee(Transfer transfer, EvmFee fee);
    }
}