using System.Threading.Tasks;
using NBitcoin;

namespace ChainSafe.Gaming.SygmaClient.Types
{
    public interface IFeeHandlerRouter
    {
        public Task<EvmFee> CalculateFee(string sender, uint fromDomainID, uint destinationDomainID, string resourceID, bytes calldata depositData, bytes calldata feeData) external view returns(uint256, address);
    }
}