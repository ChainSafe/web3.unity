using System.Threading.Tasks;
using NBitcoin;

namespace ChainSafe.Gaming.SygmaClient.Types
{
    public interface IFeeHandlerRouter
    {
        public Task<string> DomainResourceIDToFeeHandlerAddress(uint domainID, string resourceID);
    }
}