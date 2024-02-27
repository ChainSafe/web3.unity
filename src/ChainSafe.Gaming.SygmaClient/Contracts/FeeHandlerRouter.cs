using System.IO;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Contracts;
using ChainSafe.Gaming.SygmaClient.Types;

namespace ChainSafe.Gaming.SygmaClient.Contracts
{
    public class FeeHandlerRouter : IFeeHandlerRouter
    {
        private const string MethodDomainResourceIDToFeeHandlerAddress = "_domainResourceIDToFeeHandlerAddress";
        private Contract contract;

        public FeeHandlerRouter(IContractBuilder cb, string address)
        {
            var contractAbi = File.ReadAllText("Abi/FeeHandlerRouter.json");
            contract = cb.Build(contractAbi, address);
        }

        public Task<string> DomainResourceIDToFeeHandlerAddress(
            uint domainID,
            string resourceID)
        {
            var result = this.contract.Call(MethodDomainResourceIDToFeeHandlerAddress, new object[] { domainID, resourceID });
            return Task.FromResult(result.ToString());
        }
    }
}