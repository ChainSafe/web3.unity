using System.Threading.Tasks;
using Nethereum.Hex.HexTypes;

namespace ChainSafe.Gaming.SygmaClient.Types
{
    public interface IFeeHandlerRouter
    {
        public Task<string> DomainResourceIDToFeeHandlerAddress(uint domainID, HexBigInteger resourceID);
    }
}