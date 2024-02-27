using System.Threading.Tasks;

namespace ChainSafe.Gaming.SygmaClient.Types
{
    public interface IBasicHandler
    {
        Task<EvmFee> CalculateBasicFee(string sender, uint fromDomainID, uint destinationDomainID, string resourceID);
    }
}