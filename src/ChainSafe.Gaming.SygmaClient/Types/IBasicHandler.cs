using System.Threading.Tasks;
using Nethereum.Hex.HexTypes;

namespace ChainSafe.Gaming.SygmaClient.Types
{
    public interface IBasicHandler
    {
        Task<EvmFee> CalculateBasicFee(string sender, uint fromDomainID, uint destinationDomainID, HexBigInteger resourceID);
    }
}